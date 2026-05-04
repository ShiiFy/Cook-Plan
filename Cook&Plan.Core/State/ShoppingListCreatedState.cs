
namespace Cook_Plan.Core.State
{
    public class ShoppingListCreatedState : IMealPlanState
    {
        public string Name => "Список покупок создан";

        public void Generate(MealPlanContext context)
        {
            throw new InvalidOperationException("Нельзя генерировать новый план после создания списка покупок.");
        }

        public void Confirm(MealPlanContext context)
        {
            throw new InvalidOperationException("План уже подтверждён.");
        }

        public void CreateShoppingList(MealPlanContext context)
        {
            throw new InvalidOperationException("Список покупок уже создан.");
        }

        public void Complete(MealPlanContext context)
        {
            context.SetState(new CompletedPlanState());
        }
    }
}
