using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Domain.Models
{
    /// Один шаг пошаговой инструкции рецепта.
    public class RecipeStep
    {
        public int Id { get; set; }

        /// Порядковый номер шага в рецепте
        public int StepNumber { get; set; }

        /// Текстовое описание действия на данном шаге
        public string Description { get; set; } = string.Empty;

        /// Путь к иллюстрации шага
        public string? PhotoPath { get; set; }
    }
}
