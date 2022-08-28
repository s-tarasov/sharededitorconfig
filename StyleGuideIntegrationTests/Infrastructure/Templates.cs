using System.Runtime.CompilerServices;
using System.Xml.Linq;

using static StyleGuideIntegrationTests.Infrastructure.CurrentProject;

namespace StyleGuideIntegrationTests.Infrastructure
{
    internal static class Templates
    {
        public static TemplateProject Net48ModernLibrary { get; } = new TemplateProject("Net48ModernLibrary");

        public static TemplateProject NET6Library { get; } = new TemplateProject("NET6Library");

        internal class TemplateProject
        {
            public TemplateProject(string name)
            {
                Name = name;
            }

            public string ProjectFullPath => Path.Combine(SolutionDirectory, "Templates", Name, $"{Name}.csproj");

            public string TemplateClassFullPath => Path.Combine(SolutionDirectory, "Templates", Name, "ClassExample.cs");

            public string Name { get; }
        }
    }    
}
