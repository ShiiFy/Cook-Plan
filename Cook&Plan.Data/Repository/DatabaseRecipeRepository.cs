using Cook_Plan.Data.Interface;
using Cook_Plan.Domain.DBModels;
using Cook_Plan.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cook_Plan.Data.Repository
{
    public class DatabaseRecipeRepository : IRecipeRepository
    {
        private readonly AppDbContext _context;

        public DatabaseRecipeRepository(AppDbContext context)
        {
            _context = context;
        }

        public Recipe? GetRecipe(int id)
        {
            var dbRecipe = _context.Recipes
                .Include(r => r.Ingredients)
                    .ThenInclude(i => i.Product)
                .Include(r => r.Steps)
                .FirstOrDefault(r => r.Id == id);

            if (dbRecipe == null) return null;

            return MapToDomain(dbRecipe);
        }

        public List<Recipe> GetAllRecipes()
        {
            var dbRecipes = _context.Recipes
                .Include(r => r.Ingredients)
                    .ThenInclude(i => i.Product)
                .Include(r => r.Steps)
                .ToList();

            return dbRecipes.Select(MapToDomain).ToList();
        }

        // Полный маппинг всех вложенных объектов
        private Recipe MapToDomain(DBRecipe dbRecipe)
        {
            return new Recipe
            {
                Id = dbRecipe.Id,
                Name = dbRecipe.Name,
                Description = dbRecipe.Description,
                MealType = dbRecipe.MealType,
                Cuisine = dbRecipe.Cuisine,
                Difficulty = dbRecipe.Difficulty,
                CookingTimeMinutes = dbRecipe.CookingTimeMinutes,
                Servings = dbRecipe.Servings,
                PhotoPath = dbRecipe.PhotoPath,

                // Полный маппинг шагов
                Steps = dbRecipe.Steps.Select(s => new RecipeStep
                {
                    Id = s.Id,
                    StepNumber = s.StepNumber,
                    Description = s.Description,
                    PhotoPath = s.PhotoPath
                }).ToList(),

                // Полный маппинг ингредиентов и вложенных в них продуктов с БЖУ
                Ingredients = dbRecipe.Ingredients.Select(i => new Ingredient
                {
                    Id = i.Id,
                    RecipeId = i.RecipeId,
                    ProductId = i.ProductId,
                    Amount = (double)i.Amount,

                    Product = i.Product == null ? null : new Product
                    {
                        Id = i.Product.Id,
                        Name = i.Product.Name,
                        CaloriesPer100 = i.Product.Calories,
                        ProteinPer100 = i.Product.Protein,
                        FatPer100 = i.Product.Fat,
                        CarbsPer100 = i.Product.Carbohydrates,
                        Unit = i.Unit 
                    }
                }).ToList()
            };
        }
        public void AddRecipe(Recipe recipe)
        {
            var dbRecipe = new Cook_Plan.Domain.DBModels.DBRecipe
            {
                Name = recipe.Name,
                Description = recipe.Description,
                MealType = recipe.MealType,
                Cuisine = recipe.Cuisine,
                Difficulty = recipe.Difficulty,
                CookingTimeMinutes = recipe.CookingTimeMinutes,
                Servings = recipe.Servings,
                Ingredients = recipe.Ingredients.Select(i => new Cook_Plan.Domain.DBModels.DBIngredient
                {
                    Amount = (decimal)i.Amount,
                    ProductId = i.ProductId,
                    Product = i.Product != null ? new Cook_Plan.Domain.DBModels.DBProduct
                    {
                        Name = i.Product.Name,
                        Calories = i.Product.CaloriesPer100,
                        Protein = i.Product.ProteinPer100,
                        Fat = i.Product.FatPer100,
                        Carbohydrates = i.Product.CarbsPer100
                    } : null
                }).ToList(),
                Steps = recipe.Steps.Select(s => new Cook_Plan.Domain.DBModels.DBRecipeStep
                {
                    StepNumber = s.StepNumber,
                    Description = s.Description
                }).ToList()
            };

            _context.Recipes.Add(dbRecipe);
            _context.SaveChanges();

            recipe.Id = dbRecipe.Id; 
        }
        public void UpdateRecipe(Recipe recipe)
        {
            var dbRecipe = _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .FirstOrDefault(r => r.Id == recipe.Id);

            if (dbRecipe != null)
            {
                dbRecipe.Name = recipe.Name;
                dbRecipe.Description = recipe.Description;
                dbRecipe.MealType = recipe.MealType;
                dbRecipe.Cuisine = recipe.Cuisine;
                dbRecipe.Difficulty = recipe.Difficulty;
                dbRecipe.CookingTimeMinutes = recipe.CookingTimeMinutes;
                dbRecipe.Servings = recipe.Servings;

                _context.Ingredients.RemoveRange(dbRecipe.Ingredients);
                _context.RecipeSteps.RemoveRange(dbRecipe.Steps);

                dbRecipe.Ingredients = recipe.Ingredients.Select(i => new Cook_Plan.Domain.DBModels.DBIngredient
                {
                    Amount = (decimal)i.Amount,
                    ProductId = i.ProductId
                }).ToList();

                dbRecipe.Steps = recipe.Steps.Select(s => new Cook_Plan.Domain.DBModels.DBRecipeStep
                {
                    StepNumber = s.StepNumber,
                    Description = s.Description
                }).ToList();

                _context.SaveChanges();
            }
        }
        public void DeleteRecipe(int id)
        {
            var dbRecipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (dbRecipe != null)
            {
                _context.Recipes.Remove(dbRecipe);
                _context.SaveChanges();
            }
        }
    }
}
