using Cook_Plan.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Domain.Models
{
    /// Запись в плане питания. Описывает один приём пищи:
    /// какой рецепт приготовить, в какой день и в какое время суток.
    public class MealPlanEntry
    {
        public int Id { get; set; }

        /// Идентификатор плана питания, которому принадлежит запись
        public int MealPlanId { get; set; }

        /// День недели, на который запланирован приём пищи
        public DayOfWeek DayOfWeek { get; set; }

        /// Тип приёма пищи
        public MealType MealType { get; set; }

        /// Идентификатор запланированного рецепта
        public int RecipeId { get; set; }

        /// Запланированный рецепт с полными данными
        public Recipe? Recipe { get; set; }

        /// Количество порций для данного приёма пищи.
        /// Влияет на расчёт суммарного количества продуктов в списке покупок
        public int Servings { get; set; } = 1;
    }
}
