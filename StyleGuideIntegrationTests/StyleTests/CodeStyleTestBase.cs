﻿using StyleGuideIntegrationTests.Infrastructure;
using Xunit.Abstractions;

namespace StyleGuideIntegrationTests.StyleTests
{
    [Collection("CodeStyle Collection")]
    public class CodeStyleTestBase
    {
        private static CommandResult? _updatePackageResult;
        private readonly DotnetCli _dotnet;
        private readonly Templates.TemplateProject _project;

        public CodeStyleTestBase(ITestOutputHelper output)
        {
            _dotnet = new(output);
            _project = CurrentProject.Name switch 
            {
                "StyleGuideIntegrationTests" => Templates.NET6Library,
                "StyleGuideNet48IntegrationTests" => Templates.Net48ModernLibrary,
                _ => throw new ArgumentOutOfRangeException(nameof(CurrentProject.Name), CurrentProject.Name, "Unsupported Project")
            };
        }

        private protected void Run(StyleTestCase testCase)
        {
            ArgumentNullException.ThrowIfNull(testCase.FileContent);

            Assert.Equal(0, ExecuteUpdatePackage().ExitCode);

            WriteFileContent(testCase.FileContent);

            var buildResult = ExecuteBuild();

            AssertBuildResult(testCase, buildResult);

            if (!string.IsNullOrEmpty(testCase.FormattedFileContent))
            {
                ExecuteFormat();

                Assert.Contains(testCase.FormattedFileContent, GetFileContent());
            }
        }

        private static void AssertBuildResult(StyleTestCase testCase, CommandResult buildResult)
        {
            if (testCase.Errors?.Length > 0)
            {
                // Build error.
                Assert.Equal(1, buildResult.ExitCode);

                foreach (var error in testCase.Errors)
                    Assert.Contains(buildResult.OutputLines, l => l.Contains(" error") && l.Contains(error));
            }
            else
            {
                // Build success.
                Assert.Equal(0, buildResult.ExitCode);
            }

            if (testCase.Warnings?.Length > 0)
            {
                foreach (var warning in testCase.Warnings)
                    Assert.Contains(buildResult.OutputLines, l => l.Contains(" warning") && l.Contains(warning));
            }
            else
                Assert.DoesNotContain(buildResult.OutputLines, l => l.Contains(" warning"));
        }

        protected string MembersToFile(string members) => @"namespace Tests
{
    public class ClassExample
    {" + members + @"
    }
}
";

        private CommandResult ExecuteBuild() => _dotnet.Execute($@"build {_project.ProjectFullPath}");

        private CommandResult ExecuteFormat() => _dotnet.Execute($@"format {_project.ProjectFullPath}");
        private CommandResult ExecuteUpdatePackage() =>
            _updatePackageResult ??= _dotnet.Execute($@"add {_project.ProjectFullPath} package StyleGuide --prerelease");

        private string GetFileContent() => File.ReadAllText(_project.TemplateClassFullPath);

        private void WriteFileContent(string fileContent) => File.WriteAllText(_project.TemplateClassFullPath, fileContent);
    }
    internal class StyleTestCase
    {
        public string? FileContent { get; internal set; }
        public string[]? Warnings { get; internal set; }
        public string? FormattedFileContent { get; internal set; }
        public string[]? Errors { get; internal set; }
    }
}
