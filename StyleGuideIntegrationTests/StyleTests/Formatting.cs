using Xunit.Abstractions;

namespace StyleGuideIntegrationTests.StyleTests
{
    public class Formatting : CodeStyleTestBase
    {
        public Formatting(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Fields_Bad() => Run(new StyleTestCase
        {
            FileContent = MembersToFile(@"
        private  string  _a  =  """" ;

         public string A=> _a;"),
            Errors = new[] { "IDE0055: Fix formatting" },
            FormattedFileContent = MembersToFile(@"
        private string _a = """";

        public string A => _a;")
        });
    }

}
