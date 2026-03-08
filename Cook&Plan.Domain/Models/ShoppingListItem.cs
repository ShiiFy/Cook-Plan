namespace Cook_Plan.Domain.Models
{
    public class ShoppingListItem
    {
        public int Id { get; set; }

        public int ShoppingListId { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public double Amount { get; set; }
        public bool IsPurchased { get; set; }
    }
}