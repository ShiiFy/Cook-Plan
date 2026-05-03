using Cook_Plan.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cook_Plan.Domain.DBModels
{
    [Table("MealPlanEntries")]
    public class DBMealPlanEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MealPlanId { get; set; }
        [ForeignKey(nameof(MealPlanId))]
        public virtual DBMealPlan? MealPlan { get; set; }

        [Required]
        public int RecipeId { get; set; }
        [ForeignKey(nameof(RecipeId))]
        public virtual DBRecipe? Recipe { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public MealType MealType { get; set; } // Завтрак, обед, ужин
    }
}
