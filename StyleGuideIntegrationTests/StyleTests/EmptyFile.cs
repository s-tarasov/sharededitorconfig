using Xunit.Abstractions;

namespace StyleGuideIntegrationTests.StyleTests
{
    public class EmptyFile : CodeStyleTestBase
    {
        public EmptyFile(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EmptyFile_AlwaysValid() => Run(new StyleTestCase
        {
            FileContent = @"namespace Tests
{
    public class ClassExample
    {
    }
}
"
        });
    }

}
