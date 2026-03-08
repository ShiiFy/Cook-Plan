using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Prototypes
{
    public class RecipePrototype : IPrototype<Recipe>
    {
        private Recipe _recipe;
        public RecipePrototype(Recipe recipe)
        {
            _recipe = recipe;
        }
        public Recipe Clone()
        {
            return new Recipe
            {
                Name = _recipe.Name,
                Description = _recipe.Description,
                MealType = _recipe.MealType,
                Cuisine = _recipe.Cuisine,
                Difficulty = _recipe.Difficulty,
                CookingTimeMinutes = _recipe.CookingTimeMinutes,
                Servings = _recipe.Servings,
                PhotoPath = _recipe.PhotoPath,
                Ingredients = _recipe.Ingredients.Select(i => new Ingredient
                {
                    RecipeId = i.RecipeId,
                    ProductId = i.ProductId,
                    Product = i.Product,
                    Amount = i.Amount
                }).ToList(),
                Steps = _recipe.Steps.Select(s => new RecipeStep
                {
                    StepNumber = s.StepNumber,
                    Description = s.Description,
                    PhotoPath = s.PhotoPath
                }).ToList()
            };
        }
    }
}
