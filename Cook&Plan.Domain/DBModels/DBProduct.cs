using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Domain.DBModels
{
    [Table("Products")]
    public class DBProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Калорийность и БЖУ (на 100 грамм)
        [Range(0, 1000, ErrorMessage = "Калорийность не может быть отрицательной или неадекватно большой")]
        public double Calories { get; set; }

        [Range(0, 100, ErrorMessage = "Белки не могут быть меньше 0 и больше 100 грамм (на 100г продукта)")]
        public double Protein { get; set; }

        [Range(0, 100, ErrorMessage = "Жиры не могут быть меньше 0 и больше 100 грамм (на 100г продукта)")]
        public double Fat { get; set; }

        [Range(0, 100, ErrorMessage = "Углеводы не могут быть меньше 0 и больше 100 грамм (на 100г продукта)")]
        public double Carbohydrates { get; set; }
    }
}
