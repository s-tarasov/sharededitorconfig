using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using StyleGuideIntegrationTests.Infrastructure;
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
        public async Task ZeroNotConfiguredAnalyzers() 
        {
            string projectFilePath = Path.Combine(SolutionDirectory, "StyleGuide", "StyleGuide.csproj");
            var diagnosticDescriptors = await AnalyzerInspector.GetAnalyzersAsync(projectFilePath);

            _output.WriteLine($"Total {diagnosticDescriptors.Length} diagnostics found!");

            var diagnosticsConfigurations = File
                .ReadLines(Path.Combine(SolutionDirectory, "StyleGuide", "StyleCop.globalconfig"))
                .Where(l => l.StartsWith("dotnet_diagnostic"))
                .ToArray();

            _output.WriteLine("Add to analyzer config these lines:");

            var notConfiguredDiagnosticCount = 0;
            foreach (var diagnosticDescriptor in diagnosticDescriptors)
            {
                var startsWith = "dotnet_diagnostic." + diagnosticDescriptor.Id + ".";
                if (diagnosticsConfigurations.All(d => !d.StartsWith(startsWith)))
                    if (diagnosticDescriptor.IsEnabledByDefault)
                    {
                        notConfiguredDiagnosticCount++;
                        _output.WriteLine(ToEditorConfigDisableRule(diagnosticDescriptor));
                    }
            }

            Assert.Equal(0, notConfiguredDiagnosticCount);
        }

        private static string ToEditorConfigDisableRule(DiagnosticDescriptor diagnosticDescriptor)
        {
            return $"# {diagnosticDescriptor.Id}: {diagnosticDescriptor.Title}\r\ndotnet_diagnostic.{diagnosticDescriptor.Id}.severity = none\r\n";
        }
    }
}
