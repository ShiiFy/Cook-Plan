using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Enums;
using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Strategy
{
    public class CuisineRecipeStrategy : RecipeSelectionStrategyBase, IRecipeSelectionStrategy
    {
        private readonly Cuisine _cuisine;

        public string Name => $"Подбор рецептов по кухне: {_cuisine}";

        public CuisineRecipeStrategy(Cuisine cuisine)
        {
            _cuisine = cuisine;
        }

        public List<Recipe> SelectRecipes(RecipeService recipeService, int count)
        {
            var recipes = recipeService.GetAll();

            var filtered = recipes
                .Where(r => r.Cuisine == _cuisine)
                .ToList();

            if (filtered.Count == 0)
                filtered = recipes;

            return RepeatToCount(filtered, count);
        }
    }
}
