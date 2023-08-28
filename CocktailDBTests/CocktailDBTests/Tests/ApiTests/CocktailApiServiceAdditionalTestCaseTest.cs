using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using CocktailDBTests.Apis;
using CocktailDBTests.Models;
using CocktailDBTests.Logging;

namespace CocktailDBTests.Tests
{
    [TestFixture]
    public class CocktailApiServiceAdditionalTestCaseTest
    {
        private Mock<ILogger<CocktailApiService>> _loggerMock;
        private CocktailApiServiceAdditionalTestCase _additionalTestCase;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<CocktailApiService>>();
            var mockHttp = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttp.Object);
            _additionalTestCase = new CocktailApiServiceAdditionalTestCase(httpClient, _loggerMock.Object);
        }

        [Test]
        public async Task SearchNonAlcoholicIngredientsByNameAsync_ReturnsNonAlcoholicIngredient()
        {
            // Arrange
            string ingredientName = "orange juice";

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(MockApiResponseForIngredient(ingredientName)),
                });

            var httpClient = new HttpClient(mockHttp.Object);

            var capturedLogs = new List<LogEntry>(); // Create a list to capture logs
            var testLogger = new TestLogger(capturedLogs);
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddProvider(new TestLoggerProvider(testLogger));
            });

            _additionalTestCase = new CocktailApiServiceAdditionalTestCase(httpClient, loggerFactory.CreateLogger<CocktailApiService>());
            
            // Act
            var result = await _additionalTestCase.SearchNonAlcoholicIngredientsByNameAsync(ingredientName);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(0, StringComparer.OrdinalIgnoreCase.Compare("Orange juice", result[0].IngredientName));
            Assert.Null(result[0].Alcohol);
            Assert.Null(result[0].ABV);

            // Print captured logs
            Console.WriteLine($"Captured logs count: {capturedLogs.Count}");
            foreach (var log in capturedLogs)
            {
                Console.WriteLine($"Log Entry: {log.ToJsonString()}");
            }

            // Add your log assertions here
        }

        private string MockApiResponseForIngredient(string ingredientName)
        {
            return $@"{{
                ""ingredients"": [
                    {{
                        ""idIngredient"": ""123"",
                        ""strIngredient"": ""{ingredientName}"",
                        ""strDescription"": ""A delicious fruit juice"",
                        ""strType"": ""Non-alcoholic"",
                        ""strAlcohol"": null,
                        ""strABV"": null
                    }}
                ]
            }}";
        }
    }
}
