using Cook_Plan.Core.Composite;
using Cook_Plan.Core.Factories;
using Cook_Plan.Core.Iterator;
using Cook_Plan.Core.Observer;
using Cook_Plan.Core.Services;
using Cook_Plan.Core.State;
using Cook_Plan.Core.Strategy;
using Cook_Plan.Domain.Enums;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Facade
{
    public class MealPlanningFacade
    {
        private readonly RecipeService _recipeService;
        private readonly CompositeShoppingListFactory _shoppingListFactory;
        private readonly ISubject _subject;
        private readonly MealPlanContext _mealPlanContext;

        public string CurrentStateName => _mealPlanContext.CurrentStateName;

        public MealPlanningFacade(
            RecipeService recipeService,
            ISubject subject,
            MealPlanContext mealPlanContext)
        {
            _recipeService = recipeService;
            _subject = subject;
            _mealPlanContext = mealPlanContext;
            _shoppingListFactory = new CompositeShoppingListFactory();
        }

        public WeeklyMealPlanResult GenerateWeeklyPlanAndShoppingList()
        {
            return GenerateWeeklyPlanAndShoppingList(new RandomRecipeStrategy());
        }

        public WeeklyMealPlanResult GenerateWeeklyPlanAndShoppingList(IRecipeSelectionStrategy strategy)
        {
            const int daysCount = 7;
            const int mealsPerDay = 3;
            const int totalRecipesNeeded = daysCount * mealsPerDay;

            var selectedRecipes = strategy.SelectRecipes(_recipeService, totalRecipesNeeded);

            if (selectedRecipes.Count == 0)
            {
                var emptyResult = new WeeklyMealPlanResult
                {
                    Title = "Невозможно создать план",
                    StateName = _mealPlanContext.CurrentStateName,
                    PlanLines = new List<string>
                    {
                        $"Состояние: {_mealPlanContext.CurrentStateName}",
                        $"Стратегия: {strategy.Name}",
                        "Подходящие рецепты не найдены."
                    },
                    ShoppingListLines = new List<string>
                    {
                        "Список покупок не создан."
                    }
                };

                _subject.Notify(new CookPlanNotification(
                    CookPlanEventType.MealPlanChanged,
                    "План питания не был создан, потому что рецепты не найдены.",
                    emptyResult));

                return emptyResult;
            }

            _mealPlanContext.Generate();

            var week = new WeeklyPlan("План питания на неделю");

            var days = new[]
            {
                "Понедельник",
                "Вторник",
                "Среда",
                "Четверг",
                "Пятница",
                "Суббота",
                "Воскресенье"
            };

            var planLines = new List<string>
            {
                $"Состояние: {_mealPlanContext.CurrentStateName}",
                $"Стратегия подбора: {strategy.Name}"
            };

            int recipeIndex = 0;

            for (int i = 0; i < days.Length; i++)
            {
                var day = new DailyPlan(days[i]);

                var breakfast = selectedRecipes[recipeIndex++ % selectedRecipes.Count];
                var lunch = selectedRecipes[recipeIndex++ % selectedRecipes.Count];
                var dinner = selectedRecipes[recipeIndex++ % selectedRecipes.Count];

                day.Add(new CompositeRecipe(breakfast));
                day.Add(new CompositeRecipe(lunch));
                day.Add(new CompositeRecipe(dinner));

                week.Add(day);

                planLines.Add($"{days[i]}:");
                planLines.Add($"  Завтрак: {breakfast.Name}");
                planLines.Add($"  Обед: {lunch.Name}");
                planLines.Add($"  Ужин: {dinner.Name}");
            }

            var shoppingList = _shoppingListFactory.Create(week);

            var ingredientIterator = new IngredientIterator(week);
            var allIngredients = new List<Ingredient>();

            for (ingredientIterator.First(); !ingredientIterator.IsDone(); ingredientIterator.Next())
            {
                allIngredients.Add(ingredientIterator.CurrentItem());
            }

            var shoppingListLines = shoppingList.Items
                .Select(item =>
                {
                    var ingredient = allIngredients.FirstOrDefault(i => i.ProductId == item.ProductId);
                    var productName = ingredient?.Product?.Name ?? $"Продукт #{item.ProductId}";
                    var unit = ingredient?.Product?.Unit ?? "";

                    return $"• {productName} — {item.Amount} {unit}";
                })
                .ToList();

            var result = new WeeklyMealPlanResult
            {
                Title = "Фасад сгенерировал недельный план и список покупок",
                StateName = _mealPlanContext.CurrentStateName,
                WeeklyPlan = week,
                PlanLines = planLines,
                ShoppingList = shoppingList,
                ShoppingListLines = shoppingListLines
            };

            _mealPlanContext.SetCurrentPlan(result);

            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.MealPlanChanged,
                $"План питания был обновлён. Состояние: {_mealPlanContext.CurrentStateName}.",
                result));

            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.ShoppingListChanged,
                "Список покупок был обновлён.",
                shoppingList));

            return result;
        }

        public void ConfirmCurrentPlan()
        {
            _mealPlanContext.Confirm();

            NotifyStateChanged("План питания подтверждён.");
        }

        public void CreateShoppingListForCurrentPlan()
        {
            _mealPlanContext.CreateShoppingList();

            NotifyStateChanged("Список покупок создан.");
        }

        public void CompleteCurrentPlan()
        {
            _mealPlanContext.Complete();

            NotifyStateChanged("План питания выполнен.");
        }

        private void NotifyStateChanged(string message)
        {
            var currentPlan = _mealPlanContext.CurrentPlan;

            if (currentPlan != null)
            {
                currentPlan.StateName = _mealPlanContext.CurrentStateName;

                currentPlan.PlanLines.Insert(0, $"Текущее состояние: {_mealPlanContext.CurrentStateName}");
            }

            _subject.Notify(new CookPlanNotification(
                CookPlanEventType.MealPlanChanged,
                $"{message} Текущее состояние: {_mealPlanContext.CurrentStateName}.",
                currentPlan));
        }
    }
}