using Cook_Plan.Core.Facade;
using Cook_Plan.Core.Strategy;

namespace Cook_Plan.Core.Command
{
    public class GenerateMealPlanCommand : ICommandAction
    {
        private readonly MealPlanningFacade _mealPlanningFacade;
        private readonly IRecipeSelectionStrategy _strategy;
        private readonly CommandResult<WeeklyMealPlanResult> _result;

        public GenerateMealPlanCommand(
            MealPlanningFacade mealPlanningFacade,
            IRecipeSelectionStrategy strategy,
            CommandResult<WeeklyMealPlanResult> result)
        {
            _mealPlanningFacade = mealPlanningFacade;
            _strategy = strategy;
            _result = result;
        }

        public void Execute()
        {
            var plan = _mealPlanningFacade.GenerateWeeklyPlanAndShoppingList(_strategy);
            _result.SetValue(plan);
        }
    }
}
