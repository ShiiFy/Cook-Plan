using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Strategy
{
    public class AvailableIngredientsRecipeStrategy : RecipeSelectionStrategyBase, IRecipeSelectionStrategy
    {
        private readonly List<string> _availableProducts;

        public string Name => "Подбор рецептов по доступным продуктам";

        public AvailableIngredientsRecipeStrategy(IEnumerable<string> availableProducts)
        {
            _availableProducts = availableProducts
                .Select(x => x.Trim().ToLower())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        public List<Recipe> SelectRecipes(RecipeService recipeService, int count)
        {
            var recipes = recipeService.GetAll();

            var ordered = recipes
                .Select(recipe => new
                {
                    Recipe = recipe,
                    MatchCount = recipe.Ingredients.Count(i =>
                    {
                        var productName = i.Product?.Name?.ToLower() ?? "";
                        return _availableProducts.Any(p => productName.Contains(p));
                    })
                })
                .Where(x => x.MatchCount > 0)
                .OrderByDescending(x => x.MatchCount)
                .Select(x => x.Recipe)
                .ToList();

            if (ordered.Count == 0)
                ordered = recipes;

            return RepeatToCount(ordered, count);
        }
    }
}
