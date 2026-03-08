namespace Cook_Plan.Domain.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }

        public int MealPlanId { get; set; }
        public MealPlan? MealPlan { get; set; }

        public DateOnly CreatedAt { get; set; }

        public List<ShoppingListItem> Items { get; set; } = new();
    }
}