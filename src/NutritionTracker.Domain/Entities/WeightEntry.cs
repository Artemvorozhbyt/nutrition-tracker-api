namespace NutritionTracker.Domain.Entities;

public class WeightEntry
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public decimal Weight { get; set; }

    public DateOnly Date { get; set; }

    public DateTime CreatedAt { get; set; }
}