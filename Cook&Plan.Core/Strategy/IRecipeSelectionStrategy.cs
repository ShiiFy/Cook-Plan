using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Strategy
{
    public interface IRecipeSelectionStrategy
    {
        string Name { get; }

        List<Recipe> SelectRecipes(RecipeService recipeService, int count);
    }
}
