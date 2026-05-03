using Cook_Plan.Domain.Models;

namespace Cook_Plan.Data.Cache
{
    public class RecipeCache
    {
        // Потокобезопасная ленивая инициализация
        private static readonly Lazy<RecipeCache> _instance = new(() => new RecipeCache());

        public static RecipeCache Instance => _instance.Value;

        private readonly Dictionary<int, Recipe> _recipes = new();

        private RecipeCache() { }

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
        public List<Recipe> GetAll() => _recipes.Values.ToList();
    }
}
