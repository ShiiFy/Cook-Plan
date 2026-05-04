using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Adapter
{
    public class RecipeImportService
    {
        private readonly IExternalRecipeImporter _adapter;
        private readonly RecipeService _recipeService;

        public RecipeImportService(
            IExternalRecipeImporter adapter,
            RecipeService recipeService)
        {
            _adapter = adapter;
            _recipeService = recipeService;
        }

        public List<Recipe> ImportAndSaveMany(string externalJson)
        {
            var recipes = _adapter.ImportMany(externalJson);

            foreach (var recipe in recipes)
            {
                _recipeService.Add(recipe);
            }

            return recipes;
        }
    }
}
