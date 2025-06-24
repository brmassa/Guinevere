using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.ReportGenerator;
using Serilog;

namespace Guinevere.Nuke;

/// <summary>
/// This is the main build file for the project.
/// This partial is responsible for testing and coverage reporting.
/// </summary>
partial class Build
{
    private static AbsolutePath CoverageDirectory => RootDirectory / "coverage";
    private static AbsolutePath CoverageResultFile => CoverageDirectory / "coverage.xml";
    private static AbsolutePath CoverageReportDirectory => CoverageDirectory / "report";
    private static AbsolutePath CoverageReportSummaryFile => CoverageReportDirectory / "Summary.txt";

    [Parameter("Minimum coverage threshold (default: 80)")]
    public readonly int CoverageThreshold = 80;

    private Target Test => td => td
        .After(Compile)
        .Produces(CoverageResultFile)
        .Executes(() =>
            DotNetTasks.DotNetTest(settings => settings
                .SetProjectFile(Solution)
                .SetConfiguration(ConfigurationSet)

                // Test Coverage
                .SetResultsDirectory(CoverageDirectory)
                .SetCoverletOutput(CoverageResultFile)
                .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
                .SetExcludeByFile("**/*.g.cs") // Exclude source generated files
                .EnableCollectCoverage()
            )
        );

    public Target TestReport => td => td
        .DependsOn(Test)
        .Consumes(Test, CoverageResultFile)
        .Produces(CoverageReportDirectory / "**")
        .Executes(() =>
        {
            if (!CoverageResultFile.Exists())
            {
                Log.Warning("Coverage file not found: {File}", CoverageResultFile);
                return;
            }

            CoverageReportDirectory.CreateDirectory();
            
            Log.Information("Generating coverage report from {File}", CoverageResultFile);

            ReportGeneratorTasks.ReportGenerator(s => s
                .SetTargetDirectory(CoverageReportDirectory)
                .SetReportTypes(ReportTypes.Html, ReportTypes.TextSummary, ReportTypes.Badges)
                .SetReports(CoverageResultFile)
                .SetHistoryDirectory(CoverageReportDirectory / "history")
                .SetVerbosity(ReportGeneratorVerbosity.Info)
            );

            if (CoverageReportSummaryFile.Exists())
            {
                var summaryText = CoverageReportSummaryFile.ReadAllLines();
                Log.Information("Coverage Summary:");
                Log.Information(string.Join(Environment.NewLine, summaryText));

                // Check coverage threshold
                CheckCoverageThreshold(summaryText);
            }
            else
            {
                Log.Warning("Coverage summary file not found: {File}", CoverageReportSummaryFile);
            }

            Log.Information("Coverage report generated at: {Directory}", CoverageReportDirectory);
        });

    /// <summary>
    /// Checks if the coverage meets the minimum threshold
    /// </summary>
    private void CheckCoverageThreshold(string[] summaryLines)
    {
        try
        {
            foreach (var line in summaryLines)
            {
                if (line.Contains("Line coverage:") && line.Contains("%"))
                {
                    var percentageStart = line.IndexOf(": ") + 2;
                    var percentageEnd = line.IndexOf("%");
                    if (percentageStart > 1 && percentageEnd > percentageStart)
                    {
                        var percentageStr = line.Substring(percentageStart, percentageEnd - percentageStart);
                        if (double.TryParse(percentageStr, out var coverage))
                        {
                            Log.Information("Line coverage: {Coverage}%", coverage);
                            if (coverage < CoverageThreshold)
                            {
                                Log.Warning("Coverage {Coverage}% is below threshold {Threshold}%", 
                                    coverage, CoverageThreshold);
                            }
                            else
                            {
                                Log.Information("Coverage {Coverage}% meets threshold {Threshold}%", 
                                    coverage, CoverageThreshold);
                            }
                            return;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Could not parse coverage percentage from summary");
        }
    }
}
