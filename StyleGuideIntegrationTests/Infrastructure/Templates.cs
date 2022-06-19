using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace StyleGuideIntegrationTests.Infrastructure
{
    internal static class Templates
    {
        public static TemplateProject Net48ModernLibrary { get; } = new TemplateProject("Net48ModernLibrary");

        public static TemplateProject NET6Library { get; } = new TemplateProject("NET6Library");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        private static readonly string _solutionDirectory = GetCurrentProjectDirectory().Parent.FullName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8603 // Possible null reference return.
        private static DirectoryInfo GetCurrentProjectDirectory([CallerFilePath] string? filePath = null) => Directory.GetParent(Path.GetDirectoryName(filePath));
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8604 // Possible null reference argument.

        internal class TemplateProject
        {
            public TemplateProject(string name)
            {
                Name = name;
            }

            public string ProjectFullPath => Path.Combine(_solutionDirectory, "Templates", Name, $"{Name}.csproj");

            public string TemplateClassFullPath => Path.Combine(_solutionDirectory, "Templates", Name, "ClassExample.cs");

            public string Name { get; }
        }
    }    
}
