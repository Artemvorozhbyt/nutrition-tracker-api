namespace NutritionTracker.API.Contracts.Meals;

public class UpdateMealEntryRequest
{
	public Guid ProductId { get; set; }

	public decimal WeightInGrams { get; set; }
}