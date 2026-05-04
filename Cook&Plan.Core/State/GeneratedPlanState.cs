namespace Cook_Plan.Core.State
{
    public class GeneratedPlanState : IMealPlanState
    {
        public string Name => "Сгенерирован";

        public void Generate(MealPlanContext context)
        {
            context.SetState(new GeneratedPlanState());
        }

        public void Confirm(MealPlanContext context)
        {
            context.SetState(new ConfirmedPlanState());
        }

        public void CreateShoppingList(MealPlanContext context)
        {
            throw new InvalidOperationException("Сначала нужно подтвердить план.");
        }

        public void Complete(MealPlanContext context)
        {
            throw new InvalidOperationException("Нельзя завершить неподтверждённый план.");
        }
    }
}
