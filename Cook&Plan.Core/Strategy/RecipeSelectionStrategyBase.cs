using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Strategy
{
    public abstract class RecipeSelectionStrategyBase
    {
        protected double GetRecipeCalories(Recipe recipe)
        {
            return recipe.Ingredients.Sum(i => i.Calories);
        }

        protected bool ContainsExcludedProduct(Recipe recipe, IEnumerable<string> excludedProducts)
        {
            var excluded = excludedProducts
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToLower())
                .ToList();

            if (excluded.Count == 0)
                return false;

            return recipe.Ingredients.Any(i =>
            {
                var productName = i.Product?.Name?.ToLower() ?? "";
                return excluded.Any(e => productName.Contains(e));
            });
        }

        protected List<Recipe> RepeatToCount(List<Recipe> recipes, int count)
        {
            var result = new List<Recipe>();

            if (recipes.Count == 0)
                return result;

            for (int i = 0; i < count; i++)
            {
                result.Add(recipes[i % recipes.Count]);
            }

            return result;
        }
    }
}
