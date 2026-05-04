using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Command
{
    public class AddRecipeCommand : ICommandAction
    {
        private readonly RecipeService _recipeService;
        private readonly RecipeMediaService _mediaService;
        private readonly Recipe _recipe;

        public AddRecipeCommand(
            RecipeService recipeService,
            RecipeMediaService mediaService,
            Recipe recipe)
        {
            _recipeService = recipeService;
            _mediaService = mediaService;
            _recipe = recipe;
        }

        public void Execute()
        {
            _recipe.PhotoPath = _mediaService.SaveRecipePhoto(_recipe.PhotoPath);

            foreach (var step in _recipe.Steps)
            {
                step.PhotoPath = _mediaService.SaveStepPhoto(step.PhotoPath);
            }

            _recipeService.Add(_recipe);
        }
    }
}
