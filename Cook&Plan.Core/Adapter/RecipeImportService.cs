using Cook_Plan.Core.Observer;
using Cook_Plan.Core.Services;
using Cook_Plan.Domain.Enums;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Adapter
{
    public class RecipeImportService
    {
        private readonly IExternalRecipeImporter _adapter;
        private readonly RecipeService _recipeService;
        private readonly ISubject _subject;

        public RecipeImportService(
            IExternalRecipeImporter adapter,
            RecipeService recipeService,
            ISubject subject)
        {
            _adapter = adapter;
            _recipeService = recipeService;
            _subject = subject;
        }

        public List<Recipe> ImportAndSaveMany(string externalJson)
        {
            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.ImportStatusChanged,
                "Импорт рецептов начался."));

            var recipes = _adapter.ImportMany(externalJson);

            foreach (var recipe in recipes)
            {
                _recipeService.Add(recipe);
            }

            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.RecipesImported,
                $"Импортировано рецептов: {recipes.Count}",
                recipes));

            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.ImportStatusChanged,
                "Импорт рецептов завершён.",
                recipes));

            return recipes;
        }
    }
}
