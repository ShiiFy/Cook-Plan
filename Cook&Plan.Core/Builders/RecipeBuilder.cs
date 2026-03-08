using Cook_Plan.Domain.Enums;
using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Builders
{
    public class RecipeBuilder : IRecipeBuilder
    {
        Recipe _recipe = new Recipe();

        public IRecipeBuilder SetBasicInfo(string name, string? description)
        {
            _recipe.Name = name;
            _recipe.Description = description;
            return this;
        }
        public IRecipeBuilder SetDetails(MealType mealType, Cuisine cuisine, Difficulty difficulty)
        {
            _recipe.MealType = mealType;
            _recipe.Cuisine = cuisine;
            _recipe.Difficulty = difficulty;
            return this;
        }
        public IRecipeBuilder SetCookingInfo(int timeMinutes, int servings)
        {
            _recipe.CookingTimeMinutes = timeMinutes;
            _recipe.Servings = servings;
            return this;
        }
        public IRecipeBuilder AddIngredients(List<Ingredient> ingredients)
        {
            _recipe.Ingredients.AddRange(ingredients);
            return this;
        }
        public IRecipeBuilder AddIngredient(Ingredient ingredient)
        {
            _recipe.Ingredients.Add(ingredient);
            return this;
        }
        public IRecipeBuilder AddSteps(List<RecipeStep> steps)
        {
            _recipe.Steps.AddRange(steps);
            return this;
        }
        public IRecipeBuilder AddStep(RecipeStep step)
        {
            _recipe.Steps.Add(step);
            return this;
        }
        public Recipe Build()
        {
            var result = _recipe;
            _recipe = new Recipe();
            return result;
        }
    }
}
