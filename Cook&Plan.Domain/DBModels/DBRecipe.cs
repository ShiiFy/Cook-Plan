using Cook_Plan.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cook_Plan.Domain.DBModels
{
    [Table("Recipes")]
    public class DBRecipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required]
        public MealType MealType { get; set; }

        [Required]
        public Cuisine Cuisine { get; set; }

        [Required]
        public Difficulty Difficulty { get; set; }

        [Range(1, 1440)] // От 1 минуты до 24 часов
        public int CookingTimeMinutes { get; set; }

        [Range(1, 100)]
        public int Servings { get; set; }

        [MaxLength(500)]
        public string? PhotoPath { get; set; }

        // Навигационные свойства
        public virtual List<DBIngredient> Ingredients { get; set; } = new();
        public virtual List<DBRecipeStep> Steps { get; set; } = new();
    }
}
