using System.Net.Http;
using System.Threading.Tasks;
using CocktailDBTests.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CocktailDBTests.Apis
{
    public class CocktailApiServiceAdditionalTestCase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CocktailApiService> _logger;

        public CocktailApiServiceAdditionalTestCase(HttpClient httpClient, ILogger<CocktailApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IngredientModel[]> SearchNonAlcoholicIngredientsByNameAsync(string ingredientName)
        {
            try
            {
                _logger.LogInformation($"Searching for non-alcoholic ingredient: {ingredientName}");

                string baseUrl = "https://www.thecocktaildb.com/api/json/v1/1/search.php?i=";
                string apiUrl = baseUrl + ingredientName;

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return ParseJsonAndExtractFields(content);
                }
                else
                {
                    _logger.LogWarning($"API request failed with status code {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return null;
            }
        }

        private IngredientModel[] ParseJsonAndExtractFields(string jsonContent)
        {
            try
            {
                JObject jsonData = JObject.Parse(jsonContent);

                _logger.LogInformation($"Parsed JSON data: {jsonData.ToString()}");

                if (jsonData["ingredients"] != null)
                {
                    return jsonData["ingredients"].ToObject<IngredientModel[]>();
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error parsing JSON: {ex.Message}");
                return null;
            }
        }
    }
}
