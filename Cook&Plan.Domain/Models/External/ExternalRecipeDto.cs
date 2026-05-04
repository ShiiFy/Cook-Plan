using System.Text.Json.Serialization;


namespace Cook_Plan.Domain.Models.External
{
    public class ExternalRecipeDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("readyInMinutes")]
        public int ReadyInMinutes { get; set; }

        [JsonPropertyName("servings")]
        public int Servings { get; set; }

        [JsonPropertyName("cuisine")]
        public string? Cuisine { get; set; }

        [JsonPropertyName("difficulty")]
        public string? Difficulty { get; set; }

        [JsonPropertyName("mealType")]
        public string? MealType { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("ingredients")]
        public List<ExternalIngredientDto> Ingredients { get; set; } = new();

        [JsonPropertyName("steps")]
        public List<string> Steps { get; set; } = new();
    }
}
