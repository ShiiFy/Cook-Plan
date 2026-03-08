using Cook_Plan.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Domain.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public MealType MealType { get; set; }
        public Cuisine Cuisine { get; set; }
        public Difficulty Difficulty { get; set; }

        public int CookingTimeMinutes { get; set; }
        public int Servings { get; set; }

        public string? PhotoPath { get; set; }

        public List<Ingredient> Ingredients { get; set; } = new();
        public List<RecipeStep> Steps { get; set; } = new();
    }
}
