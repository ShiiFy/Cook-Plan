using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Strategy
{
    public class LowCaloriesRecipeStrategy : RecipeSelectionStrategyBase, IRecipeSelectionStrategy
    {
        private readonly double? _maxCalories;

        public string Name => "Подбор рецептов с низкой калорийностью";

        public LowCaloriesRecipeStrategy(double? maxCalories = null)
        {
            _maxCalories = maxCalories;
        }

        public List<Recipe> SelectRecipes(RecipeService recipeService, int count)
        {
            var recipes = recipeService.GetAll();

            if (_maxCalories.HasValue)
            {
                recipes = recipes
                    .Where(r => GetRecipeCalories(r) <= _maxCalories.Value)
                    .ToList();
            }

            var ordered = recipes
                .OrderBy(GetRecipeCalories)
                .ToList();

            return RepeatToCount(ordered, count);
        }
    }
}
