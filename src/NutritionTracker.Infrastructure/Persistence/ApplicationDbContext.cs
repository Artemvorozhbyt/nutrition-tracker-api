using Microsoft.EntityFrameworkCore;
using NutritionTracker.Domain.Entities;

namespace NutritionTracker.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<MealEntry> MealEntries => Set<MealEntry>();
    public DbSet<DailyGoal> DailyGoals => Set<DailyGoal>();
    public DbSet<WeightEntry> WeightEntries => Set<WeightEntry>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();
    }
}