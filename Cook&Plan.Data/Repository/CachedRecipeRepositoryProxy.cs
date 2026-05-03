using Cook_Plan.Data.Cache;
using Cook_Plan.Data.Interface;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Data.Repository
{
    public class CachedRecipeRepositoryProxy : IRecipeRepository
    {
        private readonly AppDbContext _context;
        private DatabaseRecipeRepository? _realRepository; 
        private readonly RecipeCache _cache;

        public CachedRecipeRepositoryProxy(AppDbContext context)
        {
            _context = context;
            _cache = RecipeCache.Instance; 
        }

        private DatabaseRecipeRepository GetRealRepository()
        {
            if (_realRepository == null)
            {
                _realRepository = new DatabaseRecipeRepository(_context);
            }
            return _realRepository;
        }

        public Recipe? GetRecipe(int id)
        {
            if (_cache.Contains(id))
            {
                return _cache.Get(id);
            }

            var recipe = GetRealRepository().GetRecipe(id);

            if (recipe != null)
            {
                _cache.Set(recipe);
            }

            return recipe;
        }
        public List<Recipe> GetAllRecipes()
        {
            return GetRealRepository().GetAllRecipes();
        }
        public void AddRecipe(Recipe recipe)
        {
            GetRealRepository().AddRecipe(recipe);
            _cache.Set(recipe); 
        }
        public void UpdateRecipe(Recipe recipe)
        {
            GetRealRepository().UpdateRecipe(recipe);
            _cache.Set(recipe); 
        }
        public void DeleteRecipe(int id)
        {
            GetRealRepository().DeleteRecipe(id);
            _cache.Remove(id);
        }
    }
}
