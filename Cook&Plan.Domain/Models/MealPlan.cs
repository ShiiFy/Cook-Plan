using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Domain.Models
{
    public class MealPlan
    {
        public int Id { get; set; }

        public DateOnly WeekStartDate { get; set; } // Понедельник недели

        public List<MealPlanEntry> Entries { get; set; } = new();
    }
}
