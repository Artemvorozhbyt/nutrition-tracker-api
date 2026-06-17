namespace NutritionTracker.Domain.Entities;

public class DailyGoal
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public decimal TargetCalories { get; set; }

    public decimal TargetProtein { get; set; }

    public decimal TargetFat { get; set; }

    public decimal TargetCarbs { get; set; }

    public DateTime CalculatedAt { get; set; }
}