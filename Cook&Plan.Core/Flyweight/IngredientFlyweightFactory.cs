using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Flyweight
{
    public class IngredientFlyweightFactory
    {
        private readonly Dictionary<string, Product> _products = new();
        public int Count => _products.Count;
        public Product GetProduct(
            string name,
            string unit = "г",
            double caloriesPer100 = 0,
            double proteinPer100 = 0,
            double fatPer100 = 0,
            double carbsPer100 = 0)
        {
            var key = name.Trim().ToLower();

            if (_products.TryGetValue(key, out var existing))
            {
                return existing;
            }

            var product = new Product
            {
                Name = name.Trim(),
                Unit = unit,
                CaloriesPer100 = caloriesPer100,
                ProteinPer100 = proteinPer100,
                FatPer100 = fatPer100,
                CarbsPer100 = carbsPer100
            };

            _products[key] = product;
            return product;
        }
    }
}
