using Cook_Plan.Core.Facade;

namespace Cook_Plan.Core.State
{
    public class MealPlanContext
    {
        private IMealPlanState _state;

        public WeeklyMealPlanResult? CurrentPlan { get; private set; }

        public string CurrentStateName => _state.Name;

        public MealPlanContext()
        {
            _state = new DraftPlanState();
        }

        public void SetState(IMealPlanState state)
        {
            _state = state;
        }

        public void SetCurrentPlan(WeeklyMealPlanResult plan)
        {
            CurrentPlan = plan;
        }

        public void Generate()
        {
            _state.Generate(this);
        }

        public void Confirm()
        {
            _state.Confirm(this);
        }

        public void CreateShoppingList()
        {
            _state.CreateShoppingList(this);
        }

        public void Complete()
        {
            _state.Complete(this);
        }
    }
}
