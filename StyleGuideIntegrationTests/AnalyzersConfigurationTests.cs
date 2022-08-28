using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Xunit.Abstractions;
using static StyleGuideIntegrationTests.Infrastructure.CurrentProject;

namespace StyleGuideIntegrationTests
{
    public class AnalyzersConfigurationTests
    {
        private readonly ITestOutputHelper _output;

        public AnalyzersConfigurationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task AllAnalyzersHaveExcplicitConfiguration() 
        {
            var instance = MSBuildLocator.RegisterDefaults();
            using var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (o, e) => _output.WriteLine(e.Diagnostic.Message);
            var project = await workspace.OpenProjectAsync(Path.Combine(SolutionDirectory, "StyleGuide", "StyleGuide.csproj"));
           

            // Get all analyzers in the project
            var diagnosticDescriptors = project.AnalyzerReferences
                .SelectMany(analyzerReference => analyzerReference.GetAnalyzers(LanguageNames.CSharp))
                .SelectMany(analyzer => analyzer.SupportedDiagnostics)
                .Distinct()
                .OrderBy(x => x.Id)
                .ToList();

            _output.WriteLine($"Total {diagnosticDescriptors.Count} diagnostics found!");

            var diagnosticsConfigurations = File
                .ReadLines(Path.Combine(SolutionDirectory, "StyleGuide", ".editorconfig"))
                .Where(l => l.StartsWith("dotnet_diagnostic"))
                .ToArray();

            _output.WriteLine("Missing diagnostics");

            var notConfiguredDiagnosticCount = 0;
            foreach (var diagnosticDescriptor in diagnosticDescriptors)
            {
                var startsWith = "dotnet_diagnostic." + diagnosticDescriptor.Id + ".";
                if (diagnosticsConfigurations.All(d => !d.StartsWith(startsWith)))
                    if (diagnosticDescriptor.IsEnabledByDefault)
                    {
                        notConfiguredDiagnosticCount++;
                        _output.WriteLine($"{diagnosticDescriptor.Id,-15} {diagnosticDescriptor.Title}");
                    }
            }

            Assert.Equal(0, notConfiguredDiagnosticCount);
        }
    }
}
