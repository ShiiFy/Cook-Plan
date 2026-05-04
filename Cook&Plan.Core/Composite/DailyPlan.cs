using Cook_Plan.Core.Iterator;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Composite
{
    public class DailyPlan : IMealComponent, IMealAggregate
    {
        public string DayName { get; set; }

        private readonly List<IMealComponent> _components = new();

        public int Count => _components.Count;

        public DailyPlan(string dayName)
        {
            DayName = dayName;
        }

        public void Add(IMealComponent component)
        {
            _components.Add(component);
        }

        public void Remove(IMealComponent component)
        {
            _components.Remove(component);
        }

        public IMealComponent GetChild(int index)
        {
            return _components[index];
        }

        public IIterator<IMealComponent> CreateIterator()
        {
            return new MealPlanIterator(this);
        }

        public List<Ingredient> GetIngredientsList()
        {
            return _components
                .SelectMany(c => c.GetIngredientsList())
                .GroupBy(i => i.ProductId)
                .Select(g => new Ingredient
                {
                    ProductId = g.Key,
                    Product = g.First().Product,
                    Amount = g.Sum(x => x.Amount)
                })
                .ToList();
        }
    }
}
