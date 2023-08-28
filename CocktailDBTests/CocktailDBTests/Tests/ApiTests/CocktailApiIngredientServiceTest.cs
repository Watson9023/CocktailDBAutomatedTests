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
    public class CocktailApiIngredientServiceTest
    {
        private HttpClient _httpClient;
        private CocktailApiIngredientService _apiIngredientService;
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
                    Content = new StringContent(MockApiResponseForIngredient("Vodka")),
                });

            _httpClient = new HttpClient(mockHttp.Object);
            _apiIngredientService = new CocktailApiIngredientService(_httpClient, loggerFactory.CreateLogger<CocktailApiIngredientService>());
        }

        [Test]
        public async Task SearchIngredientsByNameAsync_ValidName_LogsSearch()
        {
            // Arrange
            string ingredientName = "Vodka";
            _capturedLogs.Clear(); // Clear previous log entries

            // Act
            var result = await _apiIngredientService.SearchIngredientsByNameAsync(ingredientName);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Vodka", result[0].IngredientName);

            // Print captured logs
            Console.WriteLine($"Captured logs count: {_capturedLogs.Count}");
            foreach (var log in _capturedLogs)
            {
                Console.WriteLine($"Log Entry: {log.ToJsonString()}");
            }

            // Assert on logged messages
            Assert.AreEqual(2, _capturedLogs?.Count);
            Assert.AreEqual(LogLevel.Information, _capturedLogs[0].LogLevel);
            Assert.AreEqual($"Searching for ingredient: {ingredientName}", _capturedLogs[0].Message);

            Assert.AreEqual(LogLevel.Information, _capturedLogs[1].LogLevel);
            Assert.IsTrue(_capturedLogs[1].Message.Contains("Parsed JSON data"));
        }

        private string MockApiResponseForIngredient(string ingredientName)
        {
            return $@"{{
                ""ingredients"": [
                    {{
                        ""idIngredient"": ""1"",
                        ""strIngredient"": ""{ingredientName}"",
                        ""strDescription"": ""Vodka is a distilled beverage..."",
                        ""strType"": ""Vodka"",
                        ""strAlcohol"": ""Yes"",
                        ""strABV"": ""40""
                    }}
                ]
            }}";
        }
    }
}
