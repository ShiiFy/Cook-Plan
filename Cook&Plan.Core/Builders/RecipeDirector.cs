using Cook_Plan.Domain.Enums;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Builders
{
    public class RecipeDirector
    {
        private readonly IRecipeBuilder _builder;
    
        public RecipeDirector(IRecipeBuilder builder)
        {
            _builder = builder;
        }

        public Recipe Construct(string name, string? description, MealType mealType, Cuisine cuisine, Difficulty difficulty, int timeMinutes, int servings, List<Ingredient> ingredients, List<RecipeStep> steps)
        {
            return _builder
                .SetBasicInfo(name, description)
                .SetDetails(mealType, cuisine, difficulty)
                .SetCookingInfo(timeMinutes, servings)
                .AddIngredients(ingredients)
                .AddSteps(steps)
                .Build();
        }

    }
}
