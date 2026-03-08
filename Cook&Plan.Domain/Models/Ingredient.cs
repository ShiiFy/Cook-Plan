using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Domain.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public double Amount { get; set; }

        // Вычисляемые значения КБЖУ для данного количества
        public double Calories => Product is null ? 0 : Product.CaloriesPer100 * Amount / 100;
        public double Protein => Product is null ? 0 : Product.ProteinPer100 * Amount / 100;
        public double Fat => Product is null ? 0 : Product.FatPer100 * Amount / 100;
        public double Carbs => Product is null ? 0 : Product.CarbsPer100 * Amount / 100;
    }
}
