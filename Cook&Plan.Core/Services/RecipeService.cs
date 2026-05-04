using Cook_Plan.Core.Observer;
using Cook_Plan.Data.Interface;
using Cook_Plan.Domain.Enums;
using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Services
{
    public class RecipeService
    {
        private readonly IRecipeRepository _repository;
        private readonly ISubject _subject;

        public RecipeService(IRecipeRepository repository, ISubject subject)
        {
            _repository = repository;
            _subject = subject;
        }

        public List<Recipe> GetAll()
        {
            return _repository.GetAllRecipes();
        }

        public Recipe? GetById(int id)
        {
            return _repository.GetRecipe(id);
        }

        public void Add(Recipe recipe)
        {
            _repository.AddRecipe(recipe);

            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.RecipeAdded,
                $"Добавлен рецепт: {recipe.Name}",
                recipe));

            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.RecipesChanged,
                "Список рецептов был обновлён.",
                recipe));
        }

        public void Update(Recipe recipe)
        {
            _repository.UpdateRecipe(recipe);

            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.RecipeUpdated,
                $"Обновлён рецепт: {recipe.Name}",
                recipe));

            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.RecipesChanged,
                "Список рецептов был обновлён.",
                recipe));
        }

        public void Delete(int id)
        {
            _repository.DeleteRecipe(id);

            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.RecipeDeleted,
                $"Удалён рецепт с ID: {id}",
                id));

            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.RecipesChanged,
                "Список рецептов был обновлён.",
                id));
        }
    }
}
