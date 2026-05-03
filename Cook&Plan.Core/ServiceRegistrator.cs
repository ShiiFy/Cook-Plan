using Cook_Plan.Core.Services;
using Cook_Plan.Data;
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

            services.AddScoped<IRecipeRepository, CachedRecipeRepositoryProxy>();
            services.AddScoped<RecipeService>();

            return services;
        }

        // Вспомогательный метод для автоматических миграций (если нужно)
        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate(); // Core говорит Data создать базу
        }
    }
}
