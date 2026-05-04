
namespace Cook_Plan.Core.State
{
    public class CompletedPlanState : IMealPlanState
    {
        public string Name => "Выполнен";

        public void Generate(MealPlanContext context)
        {
            throw new InvalidOperationException("Нельзя изменить выполненный план.");
        }

        public void Confirm(MealPlanContext context)
        {
            throw new InvalidOperationException("План уже выполнен.");
        }

        public void CreateShoppingList(MealPlanContext context)
        {
            throw new InvalidOperationException("План уже выполнен.");
        }

        public void Complete(MealPlanContext context)
        {
            throw new InvalidOperationException("План уже выполнен.");
        }
    }
}
