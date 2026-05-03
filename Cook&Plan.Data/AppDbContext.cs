using Cook_Plan.Domain.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Cook_Plan.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<DBRecipe> Recipes { get; set; }
        public DbSet<DBIngredient> Ingredients { get; set; }
        public DbSet<DBProduct> Products { get; set; }
        public DbSet<DBRecipeStep> RecipeSteps { get; set; }
        public DbSet<DBMealPlan> MealPlans { get; set; }
        public DbSet<DBMealPlanEntry> MealPlanEntries { get; set; }
        public DbSet<DBShoppingList> ShoppingLists { get; set; }
        public DbSet<DBShoppingListItem> ShoppingListItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === 1. КОНВЕРТАЦИЯ ENUM В СТРОКИ ===
            modelBuilder.Entity<DBRecipe>()
                .Property(r => r.MealType).HasConversion<string>();
            modelBuilder.Entity<DBRecipe>()
                .Property(r => r.Cuisine).HasConversion<string>();
            modelBuilder.Entity<DBRecipe>()
                .Property(r => r.Difficulty).HasConversion<string>();

            modelBuilder.Entity<DBMealPlanEntry>()
                .Property(e => e.MealType).HasConversion<string>();


            // === 2. КАСКАДНОЕ УДАЛЕНИЕ (Удаляем главное -> удаляется зависимое) ===

            // Удаляем рецепт -> удаляются его ингредиенты
            modelBuilder.Entity<DBRecipe>()
                .HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Удаляем рецепт -> удаляются его шаги
            modelBuilder.Entity<DBRecipe>()
                .HasMany(r => r.Steps)
                .WithOne(s => s.Recipe)
                .HasForeignKey(s => s.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Удаляем план питания -> удаляются дни/записи из него
            modelBuilder.Entity<DBMealPlan>()
                .HasMany(mp => mp.Entries)
                .WithOne(e => e.MealPlan)
                .HasForeignKey(e => e.MealPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Удаляем список покупок -> удаляются элементы внутри него
            modelBuilder.Entity<DBShoppingList>()
                .HasMany(sl => sl.Items)
                .WithOne(i => i.ShoppingList)
                .HasForeignKey(i => i.ShoppingListId)
                .OnDelete(DeleteBehavior.Cascade);


            // === 3. ЗАЩИТА ОТ УДАЛЕНИЯ (Справочники) ===

            // Запрещаем удалять Продукт, если он есть в ингредиентах рецепта
            modelBuilder.Entity<DBIngredient>()
                .HasOne(i => i.Product)
                .WithMany() // У продукта нет списка ингредиентов (однонаправленная связь)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Запрещаем удалять Продукт, если он есть в списке покупок
            modelBuilder.Entity<DBShoppingListItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Запрещаем удалять Рецепт, если он уже добавлен в какой-то план питания
            modelBuilder.Entity<DBMealPlanEntry>()
                .HasOne(e => e.Recipe)
                .WithMany()
                .HasForeignKey(e => e.RecipeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
