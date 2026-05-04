using Cook_Plan.Core.Adapter;
using Cook_Plan.Core.Facade;
using Cook_Plan.Core.Flyweight;
using Cook_Plan.Core.Services;
using Cook_Plan.Core.Temporarily;
using Cook_Plan.Data;
using Cook_Plan.Data.Cache;
using Cook_Plan.Data.Interface;
using Cook_Plan.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cook_Plan.Core
{
    public static class ServiceRegistrator
    {
        // Метод-расширение, который UI вызовет при старте
        public static IServiceCollection AddCookPlanServices(this IServiceCollection services, string connectionString)
        {
            // 1. Core регистрирует базу данных (Data)
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Data layer
            services.AddScoped<IRecipeRepository, CachedRecipeRepositoryProxy>();
            services.AddSingleton<RecipeCache>();
            services.AddScoped<DatabaseRecipeRepository>();

            // Core layer
            services.AddSingleton<RecipeCacheManager>();
            services.AddScoped<RecipeService>();
            services.AddSingleton<IngredientFlyweightFactory>();
            services.AddScoped<MealPlanningFacade>();
            services.AddScoped<IExternalRecipeImporter, ExternalApiRecipeAdapter>();
            services.AddScoped<RecipeImportService>();

            return services;
        }

        // Вспомогательный метод для автоматических миграций
        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate(); // Core говорит Data создать базу
        }
    }
}
