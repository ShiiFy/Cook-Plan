using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cook_Plan.Domain.DBModels
{
    [Table("ShoppingListItems")]
    public class DBShoppingListItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ShoppingListId { get; set; }
        [ForeignKey(nameof(ShoppingListId))]
        public virtual DBShoppingList? ShoppingList { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual DBProduct? Product { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Unit { get; set; } = string.Empty;

        public bool IsBought { get; set; }
    }
}
