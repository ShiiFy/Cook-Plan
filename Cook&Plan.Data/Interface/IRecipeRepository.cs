using Cook_Plan.Domain.Models;

namespace Cook_Plan.Data.Interface
{
    public interface IRecipeRepository
    {
        Recipe? GetRecipe(int id);
        List<Recipe> GetAllRecipes();
        void AddRecipe(Recipe recipe);
        void UpdateRecipe(Recipe recipe); 
        void DeleteRecipe(int id);
    }
}
