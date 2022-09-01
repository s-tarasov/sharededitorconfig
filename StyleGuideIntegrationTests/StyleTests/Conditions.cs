using Xunit.Abstractions;

namespace StyleGuideIntegrationTests.StyleTests
{
    public class Conditions : CodeStyleTestBase
    {
        public Conditions(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void SingleLineIf_Bad() => Run(new StyleTestCase
        {
            FileContent = MembersToFile(@"
        public void Method1()
        {
            if (true) return;
        }"),
            Errors = new[] { "SA1501: Statement should not be on a single line" },
            FormattedFileContent = MembersToFile(@"
        public void Method1()
        {
            if (true)
                return;
        }")
        });

        [Fact]
        public void SingleLineIf_Good() => Run(new StyleTestCase
        {
            FileContent = MembersToFile(@"
        public void Method1()
        {
            if (true)
                return;
        }

        public void Method2()
        {
            if (true)
            {
                return;
            }
        }")
        });
    }
}
