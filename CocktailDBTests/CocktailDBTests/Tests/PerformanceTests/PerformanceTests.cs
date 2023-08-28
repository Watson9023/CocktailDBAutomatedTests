using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace Tests.PerformanceTests
{
    [TestFixture]
    public class PerformanceTests
    {
        [Test]
        public void RunArtilleryLoadTest()
        {
            // Get the current directory of the test
            var projectDirectory = TestContext.CurrentContext.TestDirectory;

            // Construct the path to the bash script
            var bashScriptPath = Path.Combine(projectDirectory, "run_artillery_test.sh");

            // Create a new process to run the bash script
            var process = new Process();
            process.StartInfo.FileName = "bash";
            process.StartInfo.Arguments = bashScriptPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            // Start the process
            process.Start();

            // Capture and log standard error
            string standardError = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(standardError))
            {
                Console.WriteLine($"Error output: {standardError}");
            }

            // Wait for the process to exit
            process.WaitForExit();

            // Check the exit code of the process
            if (process.ExitCode != 0)
            {
                var errorMessage = $"Artillery execution failed with error code {process.ExitCode}.";
                if (!string.IsNullOrEmpty(standardError))
                {
                    errorMessage += $" Error output: {standardError}";
                }
                throw new Exception(errorMessage);
            }

            Console.WriteLine("Artillery test completed.");
        }
    }
}