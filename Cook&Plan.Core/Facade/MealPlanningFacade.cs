using Cook_Plan.Core.Composite;
using Cook_Plan.Core.Factories;
using Cook_Plan.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Facade
{
    public class MealPlanningFacade
    {
        private readonly RecipeService _recipeService;
        private readonly CompositeShoppingListFactory _shoppingListFactory;

        public MealPlanningFacade(RecipeService recipeService)
        {
            _recipeService = recipeService;
            _shoppingListFactory = new CompositeShoppingListFactory();
        }

        public WeeklyMealPlanResult GenerateWeeklyPlanAndShoppingList()
        {
            var recipes = _recipeService.GetAll();

            if (recipes.Count == 0)
            {
                return new WeeklyMealPlanResult
                {
                    Title = "Невозможно создать план",
                    PlanLines = new List<string> { "В базе данных нет рецептов." },
                    ShoppingListLines = new List<string> { "Список покупок не создан." }
                };
            }

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

            var planLines = new List<string>();

            for (int i = 0; i < days.Length; i++)
            {
                var day = new DailyPlan(days[i]);

                var breakfast = recipes[i % recipes.Count];
                var lunch = recipes[(i + 1) % recipes.Count];
                var dinner = recipes[(i + 2) % recipes.Count];

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

            var allIngredients = recipes
                .SelectMany(r => r.Ingredients)
                .ToList();

            var shoppingListLines = shoppingList.Items
                .Select(item =>
                {
                    var ingredient = allIngredients.FirstOrDefault(i => i.ProductId == item.ProductId);
                    var productName = ingredient?.Product?.Name ?? $"Продукт #{item.ProductId}";
                    var unit = ingredient?.Product?.Unit ?? "";

                    return $"• {productName} — {item.Amount} {unit}";
                })
                .ToList();

            return new WeeklyMealPlanResult
            {
                Title = "Фасад сгенерировал недельный план и список покупок",
                PlanLines = planLines,
                ShoppingList = shoppingList,
                ShoppingListLines = shoppingListLines
            };
        }
    }
}
