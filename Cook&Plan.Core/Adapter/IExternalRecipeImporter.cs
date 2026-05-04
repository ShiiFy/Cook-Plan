using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Adapter
{
    public interface IExternalRecipeImporter
    {
        List<Recipe> ImportMany(string externalJson);
    }
}
