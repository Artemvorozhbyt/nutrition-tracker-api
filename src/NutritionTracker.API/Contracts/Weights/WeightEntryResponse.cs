namespace NutritionTracker.API.Contracts.Weights;

public class WeightEntryResponse
{
	public Guid Id { get; set; }

	public decimal Weight { get; set; }

	public DateOnly Date { get; set; }
}