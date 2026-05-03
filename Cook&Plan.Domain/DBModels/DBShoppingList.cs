using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cook_Plan.Domain.DBModels
{
    [Table("ShoppingLists")]
    public class DBShoppingList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public bool IsCompleted { get; set; }

        public virtual List<DBShoppingListItem> Items { get; set; } = new();
    }
}
