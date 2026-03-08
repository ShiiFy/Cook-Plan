using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Domain.Models
{
    /// Продукт из справочника. Хранит название, единицу измерения
    /// и пищевую ценность на 100 единиц (г / мл / шт).
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        /// Единица измерения продукта: г, мл или шт
        public string? Unit { get; set; }

        /// Калорийность на 100 единиц (ккал)
        public double CaloriesPer100 { get; set; }

        /// Содержание белков на 100 единиц (г)
        public double ProteinPer100 { get; set; }

        /// Содержание жиров на 100 единиц (г)
        public double FatPer100 { get; set; }

        /// Содержание углеводов на 100 единиц (г)
        public double CarbsPer100 { get; set; }
    }
}
