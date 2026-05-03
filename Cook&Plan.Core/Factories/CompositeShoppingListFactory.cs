using Cook_Plan.Core.Composite;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Factories
{
    public class CompositeShoppingListFactory : ShoppingListFactory<IMealComponent>
    {
        public override ShoppingList Create(IMealComponent source)
        {
            var ingredients = source.GetIngredientsList();

            var shoppingList = new ShoppingList
            {
                CreatedAt = DateOnly.FromDateTime(DateTime.Today)
            };

            shoppingList.Items = ingredients
                .GroupBy(i => i.ProductId)
                .Select(g => new ShoppingListItem
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
