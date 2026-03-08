using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Unit { get; set; } // г, мл, шт

        public double CaloriesPer100 { get; set; }
        public double ProteinPer100 { get; set; }
        public double FatPer100 { get; set; }
        public double CarbsPer100 { get; set; }
    }
}
