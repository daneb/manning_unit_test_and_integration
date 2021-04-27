using System.Diagnostics;

namespace ShoppingCartService.Test.Fixtures
{
    public static class MongoSetup
    {
        public static void Start()
        {
            string arguments = "start mongo";
            Run(arguments);
        }

        public static void Stop()
        {
            string arguments = "stop mongo";
            Run(arguments);
        }

        private static void Run(string arguments)
        {
            
            
            var processInfo = new ProcessStartInfo("docker-compose", arguments)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = new Process {StartInfo = processInfo};

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit(1200000);
            if (!process.HasExited)
            {
                process.Kill();
            }

            process.Close();
        }

        private enum ServiceControl
        {
            Start,
            Stop
        }
    }
}