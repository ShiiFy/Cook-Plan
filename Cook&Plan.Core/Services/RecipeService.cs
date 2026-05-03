using Cook_Plan.Data.Interface;
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

        public RecipeService(IRecipeRepository repository)
        {
            _repository = repository;
        }

        public List<Recipe> GetAll() => _repository.GetAllRecipes();

        public Recipe? GetById(int id) => _repository.GetRecipe(id);

        public void Add(Recipe recipe) => _repository.AddRecipe(recipe);

        public void Update(Recipe recipe) => _repository.UpdateRecipe(recipe);

        public void Delete(int id) => _repository.DeleteRecipe(id);
    }
}
