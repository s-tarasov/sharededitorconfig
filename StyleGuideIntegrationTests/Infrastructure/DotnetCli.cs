using System.Diagnostics;
using Xunit.Abstractions;

namespace StyleGuideIntegrationTests.Infrastructure
{
    internal class DotnetCli
    {
        private const string DotnetPath = "C:\\Program Files\\dotnet\\dotnet.exe";
        private readonly ITestOutputHelper _outputHelper;

        public DotnetCli(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public CommandResult Execute(string arguments)
        {
            var processOutputLines = new List<string>();
            using var buildProcess = new Process();
            buildProcess.StartInfo.FileName = DotnetPath;
            buildProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            buildProcess.StartInfo.CreateNoWindow = true;
            buildProcess.StartInfo.RedirectStandardOutput = true;
            buildProcess.StartInfo.Arguments = arguments;
            buildProcess.Start();
            while (!buildProcess.StandardOutput.EndOfStream)
            {
                var line = buildProcess.StandardOutput.ReadLine();
                processOutputLines.Add(line ?? string.Empty);
                _outputHelper.WriteLine(line ?? string.Empty);
            }
            buildProcess.WaitForExit();

            return new CommandResult(buildProcess.ExitCode, processOutputLines);
        }
    }

    public record CommandResult(int ExitCode, IReadOnlyList<string> OutputLines);
}
