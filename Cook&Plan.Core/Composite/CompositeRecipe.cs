using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Composite
{
    public class CompositeRecipe : IMealComponent
    {
        public string Name { get; set; }

        private readonly List<Ingredient> _ingredients = new();

        public CompositeRecipe(Recipe recipe)
        {
            Name = recipe.Name;

            _ingredients = recipe.Ingredients
                .Select(i => new Ingredient
                {
                    ProductId = i.ProductId,
                    Product = i.Product,
                    Amount = i.Amount
                })
                .ToList();
        }

        public List<Ingredient> GetIngredientsList()
        {
            return new List<Ingredient>(_ingredients);
        }

        public void Add(IMealComponent component)
        {
            throw new NotSupportedException();
        }

        public void Remove(IMealComponent component)
        {
            throw new NotSupportedException();
        }

        public IMealComponent GetChild(int index)
        {
            throw new NotSupportedException();
        }
    }
}
