using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Cook_Plan.Domain.DBModels
{
    [Table("RecipeSteps")]
    public class DBRecipeStep
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RecipeId { get; set; }
        [ForeignKey(nameof(RecipeId))]
        public virtual DBRecipe? Recipe { get; set; }

        [Required]
        public int StepNumber { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? PhotoPath { get; set; }
    }
}
