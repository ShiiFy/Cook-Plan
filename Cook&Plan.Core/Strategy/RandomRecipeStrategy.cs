using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Strategy
{
    public class RandomRecipeStrategy : RecipeSelectionStrategyBase, IRecipeSelectionStrategy
    {
        public string Name => "Случайный подбор рецептов";

        public List<Recipe> SelectRecipes(RecipeService recipeService, int count)
        {
            var recipes = recipeService.GetAll();

            var shuffled = recipes
                .OrderBy(_ => Guid.NewGuid())
                .ToList();

            return RepeatToCount(shuffled, count);
        }
    }
}
