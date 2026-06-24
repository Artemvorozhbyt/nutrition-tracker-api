namespace NutritionTracker.API.Contracts.Weights;

public class WeightHistoryResponse
{
    public decimal CurrentWeight { get; set; }

    public decimal? StartWeight { get; set; }

    public decimal? Difference { get; set; }

    public List<WeightEntryResponse> Entries { get; set; } = [];
}