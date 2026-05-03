using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cook_Plan.Domain.DBModels
{
    [Table("Ingredients")]
    public class DBIngredient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RecipeId { get; set; }
        [ForeignKey(nameof(RecipeId))]
        public virtual DBRecipe? Recipe { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual DBProduct? Product { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")] // Для точного веса
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Unit { get; set; } = string.Empty; // граммы, мл, шт.
    }
}
