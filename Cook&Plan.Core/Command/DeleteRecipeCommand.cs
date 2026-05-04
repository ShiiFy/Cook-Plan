using Cook_Plan.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Command
{
    public class DeleteRecipeCommand : ICommandAction
    {
        private readonly RecipeService _recipeService;
        private readonly int _recipeId;

        public DeleteRecipeCommand(RecipeService recipeService, int recipeId)
        {
            _recipeService = recipeService;
            _recipeId = recipeId;
        }

        public void Execute()
        {
            _recipeService.Delete(_recipeId);
        }
    }
}
