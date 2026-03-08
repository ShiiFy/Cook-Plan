using Cook_Plan.Domain.Enums;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Builders
{
    public interface IRecipeBuilder
    {
        IRecipeBuilder SetBasicInfo(string name, string? description); 
        IRecipeBuilder SetDetails(MealType mealType, Cuisine cuisine, Difficulty difficulty);
        IRecipeBuilder SetCookingInfo(int timeMinutes, int servings);
        IRecipeBuilder AddIngredients(List<Ingredient> ingredients);
        IRecipeBuilder AddIngredient(Ingredient ingredient);
        IRecipeBuilder AddSteps(List<RecipeStep> steps);
        IRecipeBuilder AddStep(RecipeStep step);
        Recipe Build();
    }
}
