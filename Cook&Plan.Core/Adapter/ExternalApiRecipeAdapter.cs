using Cook_Plan.Core.Builders;
using Cook_Plan.Core.Flyweight;
using Cook_Plan.Domain.Enums;
using Cook_Plan.Domain.Models;
using Cook_Plan.Domain.Models.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Adapter
{
    public class ExternalApiRecipeAdapter : IExternalRecipeImporter
    {
        private readonly IngredientFlyweightFactory _ingredientFactory;

        public ExternalApiRecipeAdapter(IngredientFlyweightFactory ingredientFactory)
        {
            _ingredientFactory = ingredientFactory;
        }

        public List<Recipe> ImportMany(string externalJson)
        {
            try
            {
                // Пытаемся распарсить как массив
                var externalRecipes = JsonSerializer.Deserialize<List<ExternalRecipeDto>>(externalJson);

                if (externalRecipes != null)
                    return externalRecipes.Select(ConvertToRecipe).ToList();
            }
            catch
            {
                // игнор — пробуем как одиночный объект
            }

            // Если не массив — пробуем один рецепт
            var single = JsonSerializer.Deserialize<ExternalRecipeDto>(externalJson);

            if (single == null)
                throw new Exception("Не удалось прочитать внешний рецепт.");

            return new List<Recipe> { ConvertToRecipe(single) };
        }
        private Recipe ConvertToRecipe(ExternalRecipeDto externalRecipe)
        {
            var ingredients = ConvertIngredients(externalRecipe);
            var steps = ConvertSteps(externalRecipe);

            var recipe = new RecipeBuilder()
                .SetBasicInfo(
                    externalRecipe.Title,
                    externalRecipe.Summary ?? "Импортированный рецепт")
                .SetDetails(
                    ConvertMealType(externalRecipe.MealType),
                    ConvertCuisine(externalRecipe.Cuisine),
                    ConvertDifficulty(externalRecipe.Difficulty))
                .SetCookingInfo(
                    externalRecipe.ReadyInMinutes > 0 ? externalRecipe.ReadyInMinutes : 30,
                    externalRecipe.Servings > 0 ? externalRecipe.Servings : 1)
                .AddIngredients(ingredients)
                .AddSteps(steps)
                .Build();

            recipe.PhotoPath = externalRecipe.Image;

            return recipe;
        }

        private List<Ingredient> ConvertIngredients(ExternalRecipeDto externalRecipe)
        {
            var result = new List<Ingredient>();

            int productId = 1;

            foreach (var item in externalRecipe.Ingredients)
            {
                var product = _ingredientFactory.GetProduct(
                    name: NormalizeIngredientName(item.OriginalName),
                    unit: string.IsNullOrWhiteSpace(item.Unit) ? "г" : item.Unit);

                result.Add(new Ingredient
                {
                    ProductId = productId++,
                    Product = product,
                    Amount = item.Amount > 0 ? item.Amount : 1
                });
            }

            return result;
        }

        private List<RecipeStep> ConvertSteps(ExternalRecipeDto externalRecipe)
        {
            var result = new List<RecipeStep>();

            for (int i = 0; i < externalRecipe.Steps.Count; i++)
            {
                result.Add(new RecipeStep
                {
                    StepNumber = i + 1,
                    Description = externalRecipe.Steps[i]
                });
            }

            return result;
        }

        private string NormalizeIngredientName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Неизвестный ингредиент";

            return name.Trim();
        }

        private MealType ConvertMealType(string? value)
        {
            return value?.ToLower() switch
            {
                "breakfast" => MealType.Breakfast,
                "lunch" => MealType.Lunch,
                "dinner" => MealType.Dinner,
                "snack" => MealType.Snack,
                "dessert" => MealType.Dessert,
                _ => MealType.Lunch
            };
        }

        private Cuisine ConvertCuisine(string? value)
        {
            return value?.ToLower() switch
            {
                "italian" => Cuisine.Italian,
                "chinese" => Cuisine.Chinese,
                "mexican" => Cuisine.Mexican,
                "french" => Cuisine.French,
                "japanese" => Cuisine.Japanese,
                "american" => Cuisine.American,
                "spanish" => Cuisine.Spanish,
                _ => Cuisine.Other
            };
        }

        private Difficulty ConvertDifficulty(string? value)
        {
            return value?.ToLower() switch
            {
                "easy" => Difficulty.Easy,
                "medium" => Difficulty.Medium,
                "hard" => Difficulty.Hard,
                _ => Difficulty.Easy
            };
        }
    }
}
