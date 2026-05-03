using Cook_Plan.Data.Cache;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Temporarily
{
    public static class RecipeCacheManager
    {
        public static List<Recipe> GetAllCached() => RecipeCache.Instance.GetAll();
        public static int GetCacheHashCode() => RecipeCache.Instance.GetHashCode();
    }
}
