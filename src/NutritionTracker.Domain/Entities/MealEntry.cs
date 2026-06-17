namespace NutritionTracker.Domain.Entities;

public class MealEntry
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ProductId { get; set; }

    public DateOnly Date { get; set; }

    public decimal WeightInGrams { get; set; }

    public decimal Calories { get; set; }

    public decimal Protein { get; set; }

    public decimal Fat { get; set; }

    public decimal Carbs { get; set; }

    public DateTime CreatedAt { get; set; }
}