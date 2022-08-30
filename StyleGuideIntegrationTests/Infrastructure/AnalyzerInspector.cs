using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis;

namespace StyleGuideIntegrationTests.Infrastructure
{
    internal static class AnalyzerInspector
    {
        public static async Task<DiagnosticDescriptor[]> GetAnalyzersAsync(string projectFilePath) {
            var instance = MSBuildLocator.RegisterDefaults();
            using var workspace = MSBuildWorkspace.Create();

            var project = await workspace.OpenProjectAsync(projectFilePath);

            return project.AnalyzerReferences
                .SelectMany(analyzerReference => analyzerReference.GetAnalyzers(LanguageNames.CSharp))
                .SelectMany(analyzer => analyzer.SupportedDiagnostics)
                .Distinct()
                .OrderBy(x => x.Id)
                .ToArray();
        }
    }
}
