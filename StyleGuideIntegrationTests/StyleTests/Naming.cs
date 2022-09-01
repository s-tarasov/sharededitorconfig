using Xunit.Abstractions;

namespace StyleGuideIntegrationTests.StyleTests
{
    public class Naming : CodeStyleTestBase
    {
        public Naming(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Fields_Bad() => Run(new StyleTestCase
        {
            FileContent = MembersToFile(@"
        private string a = """";

        public string A => a;"),
            Warnings = new[] { "IDE1006: Naming rule violation: Missing prefix: '_'" }
        });

        [Fact]
        public void Fields_Good() => Run(new StyleTestCase
        {
            FileContent = MembersToFile(@"
        private string _a = """";

        public string A => _a;")
        });
    }

}
