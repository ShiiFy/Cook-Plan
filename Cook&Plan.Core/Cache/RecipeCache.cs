using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Cache
{
    public class RecipeCache
    {
        private static RecipeCache? _instance;
        private RecipeCache() { }
        public static RecipeCache GetIstanse()
        {
            if(_instance == null)
            {
                _instance = new RecipeCache();
            }
            return _instance;
        }
        private Dictionary<int, Recipe> _recipes = new();
        public bool Contains(int id)
        {
            return _recipes.ContainsKey(id);
        }
        public Recipe? Get(int id)
        {
            _recipes.TryGetValue(id, out var recipe);
            return recipe;
        }
        public void Set(Recipe recipe)
        {
            _recipes[recipe.Id] = recipe;
        }
        public void Remove(int id)
        {
            _recipes.Remove(id);
        }
    }
}
