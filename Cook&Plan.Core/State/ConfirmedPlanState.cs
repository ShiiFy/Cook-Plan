namespace Cook_Plan.Core.State
{
    public class ConfirmedPlanState : IMealPlanState
    {
        public string Name => "Подтверждён";

        public void Generate(MealPlanContext context)
        {
            throw new InvalidOperationException("Нельзя заново генерировать подтверждённый план.");
        }

        public void Confirm(MealPlanContext context)
        {
            throw new InvalidOperationException("План уже подтверждён.");
        }

        public void CreateShoppingList(MealPlanContext context)
        {
            context.SetState(new ShoppingListCreatedState());
        }

        public void Complete(MealPlanContext context)
        {
            throw new InvalidOperationException("Сначала нужно создать список покупок.");
        }
    }
}
