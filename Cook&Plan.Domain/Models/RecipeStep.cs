using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Domain.Models
{
    public class RecipeStep
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? PhotoPath { get; set; }
    }
}
