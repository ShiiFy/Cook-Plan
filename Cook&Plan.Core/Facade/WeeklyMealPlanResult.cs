using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Facade
{
    public class WeeklyMealPlanResult
    {
        public string Title { get; set; } = string.Empty;

        public List<string> PlanLines { get; set; } = new();

        public ShoppingList ShoppingList { get; set; } = new();

        public List<string> ShoppingListLines { get; set; } = new();
    }
}
