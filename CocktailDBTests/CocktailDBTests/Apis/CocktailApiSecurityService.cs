using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace CocktailDBTests.Apis
{
    public class CocktailApiSecurityService
    {
        private const string ZAPPath = @"/System/Volumes/Data/Applications/OWASP ZAP.app/Contents/Java/zap.sh";
        private const string ZAPApiUrl = "http://localhost:8080";
        private const string TargetUrl = "https://www.thecocktaildb.com/api/json/v2/1/search.php?s=margarita";

        private readonly ILogger<CocktailApiSecurityService> _logger;

        public CocktailApiSecurityService(ILogger<CocktailApiSecurityService> logger)
        {
            _logger = logger;
        }

        public void RunSecurityScan()
        {
            _logger.LogInformation("Starting security scan...");

            StartZAP();
            StartSecurityScan();
            GenerateReport();

            _logger.LogInformation("Security scan completed.");
        }

        private void StartZAP()
        {
            _logger.LogInformation("Starting ZAP...");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ZAPPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            string outputPath = Path.Combine("/Users/watsonmatunhire/RiderProjects/CocktailDBAutomatedTests/CocktailDBTests/CocktailDBTests/OwaspZapTestsOutput", "ZapTestOutput.txt");
            string outputDir = Path.GetDirectoryName(outputPath);

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                File.WriteAllText(outputPath, output); // Save the output to the specified file

                _logger.LogInformation($"ZAP process output: {output}");
                _logger.LogError($"ZAP process error: {error}");
            }
        }

        private void StartSecurityScan()
        {
            _logger.LogInformation("Starting security scan...");

            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{ZAPApiUrl}/JSON/spider/action/scan/?url={TargetUrl}").Result;
                response.EnsureSuccessStatusCode();
            }
        }

        private void GenerateReport()
        {
            _logger.LogInformation("Generating report...");

            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{ZAPApiUrl}/OTHER/core/other/htmlreport/").Result;
                response.EnsureSuccessStatusCode();

                string reportHtml = response.Content.ReadAsStringAsync().Result;
                File.WriteAllText("security_report.html", reportHtml);
            }
        }
    }
}
