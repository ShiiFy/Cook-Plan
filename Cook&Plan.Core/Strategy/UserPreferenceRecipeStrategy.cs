using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Enums;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Strategy
{
    public class UserPreferenceRecipeStrategy : RecipeSelectionStrategyBase, IRecipeSelectionStrategy
    {
        private readonly Cuisine? _cuisine;
        private readonly double? _maxCalories;
        private readonly List<string> _excludedProducts;
        private readonly bool _preferFastRecipes;

        public string Name => "Подбор рецептов по предпочтениям пользователя";

        public UserPreferenceRecipeStrategy(
            Cuisine? cuisine = null,
            double? maxCalories = null,
            IEnumerable<string>? excludedProducts = null,
            bool preferFastRecipes = false)
        {
            _cuisine = cuisine;
            _maxCalories = maxCalories;
            _excludedProducts = excludedProducts?.ToList() ?? new List<string>();
            _preferFastRecipes = preferFastRecipes;
        }

        public List<Recipe> SelectRecipes(RecipeService recipeService, int count)
        {
            var allRecipes = recipeService.GetAll();

            var query = allRecipes.AsEnumerable();

            if (_cuisine.HasValue)
            {
                query = query.Where(r => r.Cuisine == _cuisine.Value);
            }

            if (_maxCalories.HasValue)
            {
                query = query.Where(r => GetRecipeCalories(r) <= _maxCalories.Value);
            }

            if (_excludedProducts.Count > 0)
            {
                query = query.Where(r => !ContainsExcludedProduct(r, _excludedProducts));
            }

            if (_preferFastRecipes)
            {
                query = query.OrderBy(r => r.CookingTimeMinutes);
            }
            else
            {
                query = query.OrderBy(r => GetRecipeCalories(r));
            }

            var filtered = query.ToList();

            if (filtered.Count == 0)
            {
                filtered = allRecipes
                    .Where(r => !ContainsExcludedProduct(r, _excludedProducts))
                    .OrderBy(r => GetRecipeCalories(r))
                    .ToList();
            }

            return RepeatToCount(filtered, count);
        }
    }
}
