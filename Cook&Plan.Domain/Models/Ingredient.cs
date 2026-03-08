using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Domain.Models
{
    /// Ингредиент рецепта. Связывает конкретный продукт из справочника с рецептом и хранит необходимое количество
    public class Ingredient
    {
        public int Id { get; set; }

        /// Идентификатор рецепта, которому принадлежит ингредиент
        public int RecipeId { get; set; }

        /// Идентификатор продукта из справочника
        public int ProductId { get; set; }

        /// Продукт из справочника с названием и пищевой ценностью
        public Product? Product { get; set; }

        /// Количество продукта в единицах, указанных в Product.Unit
        public double Amount { get; set; }

        /// Калорийность данного количества продукта (ккал)
        public double Calories => Product is null ? 0 : Product.CaloriesPer100 * Amount / 100;

        /// Содержание белков для данного количества продукта (г)
        public double Protein => Product is null ? 0 : Product.ProteinPer100 * Amount / 100;

        /// Содержание жиров для данного количества продукта (г)
        public double Fat => Product is null ? 0 : Product.FatPer100 * Amount / 100;

        /// Содержание углеводов для данного количества продукта (г)
        public double Carbs => Product is null ? 0 : Product.CarbsPer100 * Amount / 100;
    }
}
