using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Strategy
{
    public class ExcludeProductsRecipeStrategy : RecipeSelectionStrategyBase, IRecipeSelectionStrategy
    {
        private readonly List<string> _excludedProducts;

        public string Name => "Подбор рецептов с исключением продуктов";

        public ExcludeProductsRecipeStrategy(IEnumerable<string> excludedProducts)
        {
            _excludedProducts = excludedProducts.ToList();
        }

        public List<Recipe> SelectRecipes(RecipeService recipeService, int count)
        {
            var recipes = recipeService.GetAll();

            var filtered = recipes
                .Where(r => !ContainsExcludedProduct(r, _excludedProducts))
                .ToList();

            return RepeatToCount(filtered, count);
        }
    }
}
