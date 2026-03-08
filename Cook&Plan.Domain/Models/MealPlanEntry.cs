using Cook_Plan.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Domain.Models
{
    public class MealPlanEntry
    {
        public int Id { get; set; }

        public int MealPlanId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public MealType MealType { get; set; }

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        public int Servings { get; set; }
    }
}
