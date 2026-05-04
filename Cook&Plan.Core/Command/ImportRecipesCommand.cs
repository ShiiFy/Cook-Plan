using Cook_Plan.Core.Adapter;
using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Command
{
    public class ImportRecipesCommand : ICommandAction
    {
        private readonly RecipeImportService _importService;
        private readonly RecipeMediaService _mediaService;
        private readonly string _externalJson;
        private readonly CommandResult<List<Recipe>>? _result;

        public ImportRecipesCommand(
            RecipeImportService importService,
            RecipeMediaService mediaService,
            string externalJson,
            CommandResult<List<Recipe>>? result = null)
        {
            _importService = importService;
            _mediaService = mediaService;
            _externalJson = externalJson;
            _result = result;
        }

        public void Execute()
        {
            var recipes = _importService.ImportAndSaveMany(_externalJson);

            foreach (var recipe in recipes)
            {
                recipe.PhotoPath = _mediaService.SaveRecipePhoto(recipe.PhotoPath);

                foreach (var step in recipe.Steps)
                {
                    step.PhotoPath = _mediaService.SaveStepPhoto(step.PhotoPath);
                }
            }

            _result?.SetValue(recipes);
        }
    }
}
