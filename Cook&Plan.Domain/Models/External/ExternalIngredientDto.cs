using System.Text.Json.Serialization;

namespace Cook_Plan.Domain.Models.External
{
    public class ExternalIngredientDto
    {
        [JsonPropertyName("originalName")]
        public string OriginalName { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; } = "г";
    }
}
