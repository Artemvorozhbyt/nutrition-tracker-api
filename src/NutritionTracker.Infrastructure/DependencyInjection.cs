using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NutritionTracker.Infrastructure.Persistence;
using NutritionTracker.Application.Interfaces;
using NutritionTracker.Infrastructure.Repositories;
using NutritionTracker.Infrastructure.Services;

namespace NutritionTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IDailyGoalRepository, DailyGoalRepository>();

        services.AddScoped<IMealEntryRepository, MealEntryRepository>();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IWeightEntryRepository, WeightEntryRepository>();

        return services;
    }
}