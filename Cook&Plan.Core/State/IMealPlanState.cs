namespace Cook_Plan.Core.State
{
    public interface IMealPlanState
    {
        string Name { get; }

        void Generate(MealPlanContext context);
        void Confirm(MealPlanContext context);
        void CreateShoppingList(MealPlanContext context);
        void Complete(MealPlanContext context);
    }
}
