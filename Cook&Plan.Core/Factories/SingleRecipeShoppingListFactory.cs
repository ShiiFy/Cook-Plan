using Cook_Plan.Core.Composite;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Factories
{
    public class SingleRecipeShoppingListFactory : ShoppingListFactory<Recipe>
    {
        private readonly CompositeShoppingListFactory _compositeFactory = new();

        public override ShoppingList Create(Recipe recipe)
        {
            var recipeComponent = new CompositeRecipe(recipe);

            return _compositeFactory.Create(recipeComponent);
        }
    }
}
