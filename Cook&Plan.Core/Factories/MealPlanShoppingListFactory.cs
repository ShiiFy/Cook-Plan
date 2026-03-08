using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Factories
{
    public class MealPlanShoppingListFactory : ShoppingListFactory<MealPlan>
    {
        public override ShoppingList Create(MealPlan mealPlan)
        {
            var shoppingList = new ShoppingList
            {
                MealPlanId = mealPlan.Id,
                CreatedAt = DateOnly.FromDateTime(DateTime.Today)
            };

            var allIngredients = mealPlan.Entries
                .Where(e => e.Recipe != null)
                .SelectMany(e => e.Recipe!.Ingredients);

            shoppingList.Items = allIngredients
            .GroupBy(i => i.ProductId)              // группируем одинаковые продукты
            .Select(g => new ShoppingListItem       // для каждой группы один item
            {
                ProductId = g.Key,
                Amount = g.Sum(i => i.Amount),
                IsPurchased = false
            })
            .ToList();

            return shoppingList;
        }
    }
}
