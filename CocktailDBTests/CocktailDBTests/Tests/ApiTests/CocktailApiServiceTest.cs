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
    public class CocktailApiServiceTests
    {
        private Mock<ILogger<CocktailApiService>> _loggerMock;
        private CocktailApiService _apiService;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<CocktailApiService>>();
            var mockHttp = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttp.Object);
            _apiService = new CocktailApiService(httpClient, _loggerMock.Object);
        }

        [Test]
        public async Task SearchCocktailsByNameAsync_ValidName_ReturnsCocktails()
        {
            // Arrange
            string cocktailName = "Margarita";
            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(MockApiResponse(cocktailName)),
                });

            var httpClient = new HttpClient(mockHttp.Object);

            var capturedLogs = new List<LogEntry>(); // Create a list to capture logs
            var testLogger = new TestLogger(capturedLogs);
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddProvider(new TestLoggerProvider(testLogger));
            });

            var apiService = new CocktailApiService(httpClient, loggerFactory.CreateLogger<CocktailApiService>());

            // Act
            var result = await apiService.SearchCocktailsByNameAsync(cocktailName);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Margarita", result[0].Name);
            // ... continue for other assertions

            Console.WriteLine($"Test - End of SearchCocktailsByNameAsync_ValidName_ReturnsCocktails");

            // Print captured logs
            Console.WriteLine($"Captured logs count: {capturedLogs.Count}");
            foreach (var log in capturedLogs)
            {
                Console.WriteLine($"Log Entry: {log.ToJsonString()}");
            }
        }

        private string MockApiResponse(string cocktailName)
        {
            return $@"{{
                ""drinks"": [
                    {{
                        ""strDrink"": ""{cocktailName}"",
                        ""strCategory"": ""Ordinary Drink"",
                        ""strAlcoholic"": ""Alcoholic"",
                        ""strGlass"": ""Cocktail glass"",
                        ""strInstructions"": ""Rub the rim of the glass with the lime slice to make the salt stick to it..."",
                        ""strDrinkThumb"": ""https://www.thecocktaildb.com/images/media/drink/5noda61589575158.jpg"",
                        ""strIngredient1"": ""Tequila"",
                        ""strIngredient2"": ""Triple sec"",
                        ""strIngredient3"": ""Lime juice"",
                        ""strIngredient4"": ""Salt"",
                        ""strIngredient5"": null,
                        ""strIngredient6"": null,
                        ""strIngredient7"": null,
                        ""strIngredient8"": null,
                        ""strIngredient9"": null,
                        ""strIngredient10"": null,
                        ""strIngredient11"": null,
                        ""strIngredient12"": null,
                        ""strIngredient13"": null,
                        ""strIngredient14"": null,
                        ""strIngredient15"": null,
                        ""strMeasure1"": ""1 1/2 oz"",
                        ""strMeasure2"": ""1/2 oz"",
                        ""strMeasure3"": ""1 oz"",
                        ""strMeasure4"": null,
                        ""strMeasure5"": null,
                        ""strMeasure6"": null,
                        ""strMeasure7"": null,
                        ""strMeasure8"": null,
                        ""strMeasure9"": null,
                        ""strMeasure10"": null,
                        ""strMeasure11"": null,
                        ""strMeasure12"": null,
                        ""strMeasure13"": null,
                        ""strMeasure14"": null,
                        ""strMeasure15"": null,
                        ""strImageSource"": ""https://commons.wikimedia.org/wiki/File:Klassiche_Margarita.jpg"",
                        ""strImageAttribution"": ""Cocktailmarler"",
                        ""strCreativeCommonsConfirmed"": ""Yes"",
                        ""dateModified"": ""2015-08-18 14:42:59""
                    }}
                ]
            }}";
        }


    }
}
