using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Factories
{
    public class SingleRecipeShoppingListFactory : ShoppingListFactory<Recipe>
    {
        public override ShoppingList Create(Recipe recipe)
        {
            var shoppingList = new ShoppingList {CreatedAt = DateOnly.FromDateTime(DateTime.Today)};

            shoppingList.Items = recipe.Ingredients
            .Select(i => new ShoppingListItem
            {
                ProductId = i.ProductId,
                Amount = i.Amount,
                IsPurchased = false
            })
            .ToList();

            return shoppingList;
        }
    }
}
