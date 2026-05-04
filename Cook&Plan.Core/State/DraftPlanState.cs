namespace Cook_Plan.Core.State
{
    public class DraftPlanState : IMealPlanState
    {
        public string Name => "Черновик";

        public void Generate(MealPlanContext context)
        {
            context.SetState(new GeneratedPlanState());
        }

        public void Confirm(MealPlanContext context)
        {
            throw new InvalidOperationException("Нельзя подтвердить план, пока он не сгенерирован.");
        }

        public void CreateShoppingList(MealPlanContext context)
        {
            throw new InvalidOperationException("Нельзя создать список покупок из черновика.");
        }

        public void Complete(MealPlanContext context)
        {
            throw new InvalidOperationException("Нельзя завершить черновик.");
        }
    }
}
