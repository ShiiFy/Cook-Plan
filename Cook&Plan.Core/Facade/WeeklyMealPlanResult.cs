using Cook_Plan.Core.Composite;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Facade
{
    public class WeeklyMealPlanResult
    {
        public string Title { get; set; } = string.Empty;

        public string StateName { get; set; } = string.Empty;

        public WeeklyPlan? WeeklyPlan { get; set; }

        public List<string> PlanLines { get; set; } = new();

        public ShoppingList ShoppingList { get; set; } = new();

        public List<string> ShoppingListLines { get; set; } = new();
    }
}
