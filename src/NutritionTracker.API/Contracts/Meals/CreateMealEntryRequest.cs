namespace NutritionTracker.API.Contracts.Meals;

public class CreateMealEntryRequest
{
    public Guid ProductId { get; set; }

    public decimal WeightInGrams { get; set; }
}