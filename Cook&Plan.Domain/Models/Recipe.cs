using Cook_Plan.Domain.Enums;

namespace Cook_Plan.Domain.Models
{
    /// Рецепт блюда. Содержит всю информацию о блюде:
    /// описание, категорию, время приготовления, ингредиенты и пошаговую инструкцию.
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        /// Тип приёма пищи: завтрак, обед, ужин, перекус или десерт
        public MealType MealType { get; set; }

        /// Кухня мира, к которой относится блюдо
        public Cuisine Cuisine { get; set; }

        /// Сложность приготовления
        public Difficulty Difficulty { get; set; }

        /// Время приготовления в минутах
        public int CookingTimeMinutes { get; set; }

        /// Количество порций, на которое рассчитан рецепт
        public int Servings { get; set; }

        /// Путь к фотографии готового блюда на диске
        public string? PhotoPath { get; set; }

        /// Список ингредиентов с указанием продукта и его количества
        public List<Ingredient> Ingredients { get; set; } = new();

        /// Пошаговая инструкция приготовления блюда
        public List<RecipeStep> Steps { get; set; } = new();
    }
}
