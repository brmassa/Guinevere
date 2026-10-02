using System.Text.RegularExpressions;

namespace Build;

/// <summary>
/// This is the main build file for the project.
/// This partial is responsible for the versioning using GitVersion.
/// </summary>
partial class Build
{
    [GitRepository]
    private readonly GitRepository Repository;

    [GitVersion]
    private readonly GitVersion GitVersion;

    private string CachedVersionFull;

    /// <summary>
    /// The release version. GitVersion decides it; the Conventional Commits fallback only takes over when
    /// GitVersion is missing or does not land above the latest release (shallow clone, stray older tag).
    /// </summary>
    private string VersionFull => CachedVersionFull ??= ResolveVersion();

    private string ResolveVersion()
    {
        var latestRelease = CurrentFullVersion;
        if (GetCommitsSinceLastTag() == 0)
        {
            Log.Debug("HEAD is the {Tag} release", CurrentTag);
            return latestRelease;
        }

        var gitVersionValue = GitVersion?.MajorMinorPatch;
        if (gitVersionValue != null && IsVersionEarlierThan(latestRelease, gitVersionValue))
        {
            Log.Debug("Using GitVersion: {Version}", gitVersionValue);
            return gitVersionValue;
        }

        var fallbackVersion = CalculateNextVersion();
        Log.Warning("GitVersion returned {GitVersion}, which is not above release {Release}. Using {Fallback}",
            gitVersionValue ?? "nothing", latestRelease, fallbackVersion);
        return fallbackVersion;
    }

    /// <summary>
    /// The version in a format that can be used as a tag.
    /// </summary>
    private string TagName => $"v{VersionFull}";

    /// <summary>
    /// Checks if there are new commits since the last tag.
    /// </summary>
    private bool HasNewCommits =>
        GitVersion != null ? GitVersion.CommitsSinceVersionSource != "0" : GetCommitsSinceLastTag() > 0;

    private string CurrentVersion;

    /// <summary>
    /// The latest release: the highest release tag reachable from HEAD or the highest changelog version,
    /// whichever is newer. The changelog covers a release whose tag was never created.
    /// </summary>
    private string CurrentTag
    {
        get
        {
            if (CurrentVersion != null)
                return CurrentVersion;

            CurrentVersion = SelectLatestReleaseTag(LatestReachableTag, GetLatestChangelogTag()) ?? "v1.0.0";
            return CurrentVersion;
        }
    }

    private string CachedReachableTag;

    /// <summary>
    /// The highest release tag reachable from HEAD, or null before the first release. `describe` is not used
    /// because it selects the closest tag, which is wrong when a stray tag was created after a newer release.
    /// </summary>
    private string LatestReachableTag
    {
        get
        {
            if (CachedReachableTag != null)
                return CachedReachableTag.Length == 0 ? null : CachedReachableTag;

            try
            {
                CachedReachableTag = GitTasks.Git("tag --merged HEAD --sort=-version:refname", logOutput: false)
                    .Select(output => output.Text)
                    .FirstOrDefault(IsReleaseTag) ?? "";
            }
            catch
            {
                // An empty repository has no tags; the release range then covers the whole history.
                CachedReachableTag = "";
            }

            return CachedReachableTag.Length == 0 ? null : CachedReachableTag;
        }
    }

    private string CurrentFullVersion => CurrentTag.TrimStart('v');

    private static bool IsReleaseTag(string tag) =>
        tag.StartsWith('v') && Version.TryParse(tag[1..], out var version) && version.Build >= 0;

    private static bool IsVersionEarlierThan(string candidate, string baseline) =>
        Version.TryParse(candidate, out var candidateVersion) &&
        Version.TryParse(baseline, out var baselineVersion) &&
        candidateVersion < baselineVersion;

    private string GetLatestChangelogTag()
    {
        if (!File.Exists(ChangelogFile))
            return null;

        return VersionRegex().Matches(File.ReadAllText(ChangelogFile))
            .Select(match => $"v{match.Groups[1].Value}")
            .Where(IsReleaseTag)
            .OrderByDescending(tag => Version.Parse(tag[1..]))
            .FirstOrDefault();
    }

    private static string SelectLatestReleaseTag(params string[] tags) =>
        tags.Where(tag => !string.IsNullOrWhiteSpace(tag))
            .OrderByDescending(tag => Version.Parse(tag[1..]))
            .FirstOrDefault();

    /// <summary>
    /// Bumps the latest release by the largest Conventional Commits change since the last reachable tag,
    /// using the same rules as GitVersion.yml.
    /// </summary>
    private string CalculateNextVersion()
    {
        var range = LatestReachableTag is { } tag ? $"{tag}..HEAD" : "HEAD";
        var messageLines = GitTasks.Git($"log --format=%B {range}", logOutput: false).Select(output => output.Text);
        return BumpVersion(CurrentFullVersion, messageLines);
    }

    /// <summary>
    /// Returns <paramref name="version"/> bumped by the largest change in the commit message lines:
    /// major for `type!:` or `BREAKING CHANGE:`, minor for `feat:`, patch otherwise.
    /// </summary>
    internal static string BumpVersion(string version, IEnumerable<string> messageLines)
    {
        var current = Version.Parse(version);
        var bump = messageLines.Select(GetReleaseBump).DefaultIfEmpty(ReleaseBump.Patch).Max();
        return bump switch
        {
            ReleaseBump.Major => $"{current.Major + 1}.0.0",
            ReleaseBump.Minor => $"{current.Major}.{current.Minor + 1}.0",
            _ => $"{current.Major}.{current.Minor}.{current.Build + 1}",
        };
    }

    private static ReleaseBump GetReleaseBump(string line) =>
        BreakingChangeRegex().IsMatch(line) ? ReleaseBump.Major
        : FeatureRegex().IsMatch(line) ? ReleaseBump.Minor
        : ReleaseBump.Patch;

    private enum ReleaseBump
    {
        Patch,
        Minor,
        Major,
    }

    [GeneratedRegex(@"^\w+(\([^)]*\))?!:|^BREAKING[ -]CHANGE:")]
    private static partial Regex BreakingChangeRegex();

    [GeneratedRegex(@"^feat(\([^)]*\))?:")]
    private static partial Regex FeatureRegex();

    /// <summary>
    /// Gets the number of commits since the last reachable release tag.
    /// </summary>
    private int GetCommitsSinceLastTag()
    {
        try
        {
            var range = LatestReachableTag is { } tag ? $"{tag}..HEAD" : "HEAD";
            var commitCountText = GitTasks.Git($"rev-list --count {range}", logOutput: false)
                .FirstOrDefault().Text;

            if (int.TryParse(commitCountText, out var directCount))
                return directCount;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Could not calculate commits since last tag");
        }

        return 1; // Assume there are changes if we can't determine
    }

    /// <summary>
    /// Gets version from environment variables (useful for CI/CD).
    /// </summary>
    private string GetEnvironmentVersion()
    {
        // Check for GitHub Actions tag reference
        var githubRef = Environment.GetEnvironmentVariable("GITHUB_REF");
        if (!string.IsNullOrEmpty(githubRef) && githubRef.StartsWith("refs/tags/"))
        {
            var tag = githubRef.Substring("refs/tags/".Length);
            if (tag.StartsWith("v") && IsValidVersion(tag.Substring(1)))
                return tag;
        }

        return null;
    }

    /// <summary>
    /// Validates if a string is a valid semantic version.
    /// </summary>
    private bool IsValidVersion(string version)
    {
        if (string.IsNullOrEmpty(version)) return false;

        var parts = version.Split('.');
        if (parts.Length < 2 || parts.Length > 4) return false;

        return parts.Take(3).All(part => int.TryParse(part, out _));
    }

    /// <summary>
    /// Prints the current version.
    /// </summary>
    private Target ShowCurrentVersion => td => td
        .Executes(() =>
        {
            Log.Information("Current version:  {Version}", CurrentFullVersion);
            Log.Information("Current tag:      {Version}", CurrentTag);
            Log.Information("Next version:     {Version}", VersionFull);

            if (GitVersion == null)
            {
                Log.Warning("GitVersion is not available - using fallback version from git tags");
                var envVersion = GetEnvironmentVersion();
                if (envVersion != null)
                {
                    Log.Information("Environment version detected: {Version}", envVersion);
                }
            }
            else
            {
                Log.Information("GitVersion available - commits since last version: {Commits}",
                    GitVersion.CommitsSinceVersionSource);
            }
        });

    /// <summary>
    /// Checks if there are new commits since the last tag.
    /// If there are no new commits, the whole publish process is skipped.
    /// </summary>
    private Target CheckNewCommits => td => td
        .DependsOn(ShowCurrentVersion)
        .Executes(() =>
        {
            Log.Information("Next version:    {Version}", TagName);

            if (GitVersion != null)
            {
                // If there are no new commits since the last tag, skip tag creation
                // Nuke will stop here and not execute any of the following targets
                if (HasNewCommits)
                    Log.Information("There are {GitVersionCommitsSinceVersionSource} new commits since last tag",
                        GitVersion.CommitsSinceVersionSource);
                else
                    Log.Information("No new commits since last tag. Skipping tag creation");
            }
            else
            {
                Log.Warning("GitVersion not available - continuing with CI/CD pipeline");
                Log.Information("Using fallback versioning strategy");
            }
        });

    /// <summary>
    /// Stamps the release version into every project that ships a package, so the committed
    /// csproj version matches the tag. Test, build and example projects are left alone.
    /// </summary>
    private Target UpdateProjectVersions => td => td
        .DependsOn(CheckNewCommits)
        .Executes(() =>
        {
            Log.Information("Projects: {ProjectsCount}", PackageableProjects.Count);

            PackageableProjects.ForEach(project =>
            {
                if (project == null) return;
                Log.Information("{Project}:\tfrom {Version} to {VersionFull}",
                    project.Name, project.GetProperty("Version"), VersionFull);
                var msbuildProject = project.GetMSBuildProject();
                msbuildProject.SetProperty("Version", VersionFull);
                msbuildProject.Save(project.Path);
            });
        });
}
