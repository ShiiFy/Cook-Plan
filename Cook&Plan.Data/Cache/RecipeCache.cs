using Cook_Plan.Domain.Models;

namespace Cook_Plan.Data.Cache
{
    public class RecipeCache
    {
        private readonly Dictionary<int, Recipe> _recipes = new();

        public bool Contains(int id) => _recipes.ContainsKey(id);

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

        public List<Recipe> GetAll() => _recipes.Values.ToList();
    }
}
