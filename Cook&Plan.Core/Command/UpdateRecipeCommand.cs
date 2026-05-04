using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Command
{
    public class UpdateRecipeCommand : ICommandAction
    {
        private readonly RecipeService _recipeService;
        private readonly RecipeMediaService _mediaService;
        private readonly Recipe _recipe;

        public UpdateRecipeCommand(
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

            _recipeService.Update(_recipe);
        }
    }
}
