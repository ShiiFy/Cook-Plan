using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Composite
{
    public class DailyPlan : IMealComponent
    {
        public string DayName { get; set; }

        private readonly List<IMealComponent> _components = new();

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
