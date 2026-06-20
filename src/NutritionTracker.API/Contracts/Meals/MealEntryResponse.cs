namespace NutritionTracker.API.Contracts.Meals;

public class MealEntryResponse
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public decimal WeightInGrams { get; set; }

    public decimal Calories { get; set; }

    public decimal Protein { get; set; }

    public decimal Fat { get; set; }

    public decimal Carbs { get; set; }

    public DateOnly Date { get; set; }
}