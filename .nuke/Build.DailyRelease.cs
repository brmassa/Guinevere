using System;
using Nuke.Common;
using Serilog;

namespace Guinevere.Nuke;

/// <summary>
/// This is the main build file for the project.
/// This partial is responsible for the daily release automation.
/// </summary>
partial class Build
{
    [Parameter("Skip daily release if no changes found")]
    public readonly bool SkipDailyReleaseOnNoChanges = true;

    [Parameter("Force daily release even without changes")]
    public readonly bool ForceDailyRelease;

    /// <summary>
    /// Complete daily release pipeline: Test, Build, Package, Release, and Publish
    /// This target is specifically designed for automated daily releases
    /// </summary>
    private Target DailyRelease => td => td
        .OnlyWhenStatic(() => ForceDailyRelease || HasNewCommits || !SkipDailyReleaseOnNoChanges)
        .DependsOn(CheckNewCommits, CI, PackNuGet, PackageSamples, CreateTag, CreateGitHubRelease, PublishNuGet)
        .Executes(() =>
        {
            if (!HasNewCommits && !ForceDailyRelease)
            {
                Log.Information("No new commits found since last release - skipping daily release");
                return;
            }

            Log.Information("Daily release completed successfully for version {Version}", VersionFull);
            Log.Information("Release summary:");
            Log.Information("  - Version: {Version}", VersionFull);
            Log.Information("  - Tag: {Tag}", TagName);
            Log.Information("  - GitHub Release: Created");
            Log.Information("  - NuGet Packages: Published");
            Log.Information("  - Sample Packages: Created");
        });

    /// <summary>
    /// Daily CI pipeline: Clean, Restore, Compile, and Test
    /// Optimized for daily automation with comprehensive testing
    /// </summary>
    private Target DailyCI => td => td
        .DependsOn(Clean, Restore, Compile, Test, TestReport)
        .Executes(() =>
        {
            Log.Information("Daily CI pipeline completed successfully");
            Log.Information("All tests passed and code quality checks completed");
        });

    /// <summary>
    /// Daily build pipeline: Build all deliverables for release
    /// Creates packages but doesn't publish them yet
    /// </summary>
    private Target DailyBuild => td => td
        .DependsOn(DailyCI, PackNuGet, PackageSamples)
        .Executes(() =>
        {
            Log.Information("Daily build pipeline completed successfully");
            Log.Information("All deliverables built and packaged");
        });

    /// <summary>
    /// Daily publish pipeline: Publish all packages to their respective repositories
    /// Only runs if there are new commits or forced
    /// </summary>
    private Target DailyPublish => td => td
        .DependsOn(DailyBuild, CreateTag, CreateGitHubRelease, PublishNuGet)
        .OnlyWhenStatic(() => ForceDailyRelease || HasNewCommits)
        .Executes(() =>
        {
            Log.Information("Daily publish pipeline completed successfully");
            Log.Information("All packages published and GitHub release created");
        });

    /// <summary>
    /// Quick daily check: Runs basic validation without full CI/CD
    /// Useful for daily health checks
    /// </summary>
    private Target DailyCheck => td => td
        .DependsOn(ShowCurrentVersion, CheckNewCommits, Restore, Compile)
        .Executes(() =>
        {
            Log.Information("Daily check completed successfully");

            if (HasNewCommits)
            {
                Log.Information("✅ New commits detected - ready for release");
                Log.Information("Next steps: Run 'DailyRelease' target to create and publish release");
            }
            else
            {
                Log.Information("ℹ️ No new commits since last release");
                Log.Information("Current version: {Version}", VersionFull);
            }
        });

    /// <summary>
    /// Daily status report: Shows current state without building anything
    /// </summary>
    private Target DailyStatus => td => td
        .DependsOn(ShowCurrentVersion)
        .Executes(() =>
        {
            var now = DateTime.UtcNow;
            var brtTime = now.AddHours(-3); // Convert UTC to BRT (UTC-3)

            Log.Information("Daily Status Report - {Date:yyyy-MM-dd} {Time:HH:mm} BRT", brtTime.Date, brtTime);
            Log.Information("Repository: {Repo}", Repository?.HttpsUrl ?? "Unknown");
            Log.Information("Branch: {Branch}", Repository?.Branch ?? "Unknown");
            Log.Information("Current Version: {Version}", VersionFull);
            Log.Information("Has New Commits: {HasChanges}", HasNewCommits ? "Yes" : "No");

            if (GitVersion != null)
            {
                Log.Information("Commits Since Last Version: {Commits}", GitVersion.CommitsSinceVersionSource);
                Log.Information("GitVersion Info:");
                Log.Information("  - Major: {Major}", GitVersion.Major);
                Log.Information("  - Minor: {Minor}", GitVersion.Minor);
                Log.Information("  - Patch: {Patch}", GitVersion.Patch);
                Log.Information("  - PreReleaseTag: {PreReleaseTag}", GitVersion.PreReleaseTag ?? "None");
            }
            else
            {
                Log.Warning("GitVersion not available - using fallback versioning");
            }
        });
}
