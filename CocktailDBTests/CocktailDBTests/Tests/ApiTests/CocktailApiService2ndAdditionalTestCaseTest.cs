using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using CocktailDBTests.Apis;
using CocktailDBTests.Models;
using CocktailDBTests.Logging;

namespace CocktailDBTests.Tests
{
    [TestFixture]
    public class CocktailApiService2ndAdditionalTestCaseTest
    {
        private HttpClient _httpClient;
        private CocktailApiService _apiService;
        private List<LogEntry> _capturedLogs;
        private TestLogger _testLogger;

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {
            _capturedLogs = new List<LogEntry>();
            _testLogger = new TestLogger(_capturedLogs);
            
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddProvider(new TestLoggerProvider(_testLogger));
            });

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(MockApiResponseForCocktail("MaRgArItA")),
                });

            _httpClient = new HttpClient(mockHttp.Object);
            _apiService = new CocktailApiService(_httpClient, loggerFactory.CreateLogger<CocktailApiService>());
        }

        [Test]
        public async Task SearchCocktailsByNameAsync_CaseInsensitive_ReturnsCocktails()
        {
            // Arrange
            string cocktailName = "MaRgArItA";
            _capturedLogs.Clear(); // Clear previous log entries

            // Act
            var result = await _apiService.SearchCocktailsByNameAsync(cocktailName);

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(string.Equals("Margarita", result[0].Name, StringComparison.OrdinalIgnoreCase));

            // Print captured logs
            Console.WriteLine($"Captured logs count: {_capturedLogs.Count}");
            foreach (var log in _capturedLogs)
            {
                Console.WriteLine($"Log Entry: {log.ToJsonString()}");
            }
        }

        private string MockApiResponseForCocktail(string cocktailName)
        {
            return $@"{{
                ""drinks"": [
                    {{
                        ""strDrink"": ""{cocktailName}"",
                        ""strCategory"": ""Ordinary Drink"",
                        ""strAlcoholic"": ""Alcoholic"",
                        // ... rest of the cocktail data ...
                    }}
                ]
            }}";
        }
    }
}
