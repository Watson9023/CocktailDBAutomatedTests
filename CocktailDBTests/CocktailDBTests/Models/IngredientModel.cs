namespace CocktailDBTests.Models;
using Newtonsoft.Json;

public class IngredientModel
{
    [JsonProperty("idIngredient")]
    public string IngredientId { get; set; }

    [JsonProperty("strIngredient")]
    public string IngredientName { get; set; }

    [JsonProperty("strDescription")]
    public string Description { get; set; }

    [JsonProperty("strType")]
    public string Type { get; set; }

    [JsonProperty("strAlcohol")]
    public string Alcohol { get; set; }

    [JsonProperty("strABV")]
    public string ABV { get; set; }
}
