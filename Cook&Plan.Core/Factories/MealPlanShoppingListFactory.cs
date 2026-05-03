using Cook_Plan.Core.Composite;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Factories
{
    public class MealPlanShoppingListFactory : ShoppingListFactory<MealPlan>
    {
        private readonly CompositeShoppingListFactory _compositeFactory = new();

        public override ShoppingList Create(MealPlan mealPlan)
        {
            var week = new WeeklyPlan("План питания");

            foreach (var entry in mealPlan.Entries)
            {
                if (entry.Recipe == null)
                    continue;

                week.Add(new CompositeRecipe(entry.Recipe));
            }

            var shoppingList = _compositeFactory.Create(week);
            shoppingList.MealPlanId = mealPlan.Id;

            return shoppingList;
        }
    }
}
