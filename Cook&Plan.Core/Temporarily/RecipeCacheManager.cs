using Cook_Plan.Data.Cache;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Temporarily
{
    public class RecipeCacheManager
    {
        private readonly RecipeCache _recipeCache;

        public RecipeCacheManager(RecipeCache recipeCache)
        {
            _recipeCache = recipeCache;
        }

        public List<Recipe> GetAllCached()
        {
            return _recipeCache.GetAll();
        }

        public int GetCacheHashCode()
        {
            return _recipeCache.GetHashCode();
        }
    }
}
