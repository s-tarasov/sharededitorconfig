using Xunit.Abstractions;

namespace StyleGuideIntegrationTests.StyleTests
{
    public class EmptyLines : CodeStyleTestBase
    {
        public EmptyLines(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void EmptyLinesAndBraces() => Run(new StyleTestCase
        {
            FileContent = MembersToFile(@"
        public int Method() 
        {
            
            return 1;
        }"),
            Warnings = new[] { "SA1505: An opening brace should not be followed by a blank line" },
            FormattedFileContent = MembersToFile(@"
        public int Method()
        {
            return 1;
        }")
        });
    }


}
