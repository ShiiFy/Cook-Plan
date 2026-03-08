namespace Cook_Plan.Domain.Models
{
    /// Список покупок, сгенерированный на основе плана питания.
    public class ShoppingList
    {
        public int Id { get; set; }

        /// Идентификатор плана питания, на основе которого создан список
        public int MealPlanId { get; set; }

        /// План питания, на основе которого создан список
        public MealPlan? MealPlan { get; set; }

        /// Дата создания списка покупок
        public DateOnly CreatedAt { get; set; }

        /// Позиции списка с продуктами и их суммарным количеством
        public List<ShoppingListItem> Items { get; set; } = new();
    }
}