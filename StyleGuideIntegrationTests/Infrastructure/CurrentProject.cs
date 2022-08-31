using System.Runtime.CompilerServices;

namespace StyleGuideIntegrationTests.Infrastructure
{
    internal static class CurrentProject
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        public static readonly string SolutionDirectory = GetProjectDirectory().Parent.FullName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        public static readonly string Name = typeof(CurrentProject).Assembly.GetName().Name;

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8603 // Possible null reference return.
        private static DirectoryInfo GetProjectDirectory([CallerFilePath] string? filePath = null) => Directory.GetParent(Path.GetDirectoryName(filePath));
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8604 // Possible null reference argument.
    }
}
