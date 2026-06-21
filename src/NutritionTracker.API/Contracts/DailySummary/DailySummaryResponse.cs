namespace NutritionTracker.API.Contracts.DailySummary;

public class DailySummaryResponse
{
    public decimal GoalCalories { get; set; }

    public decimal GoalProtein { get; set; }

    public decimal GoalFat { get; set; }

    public decimal GoalCarbs { get; set; }

    public decimal ConsumedCalories { get; set; }

    public decimal ConsumedProtein { get; set; }

    public decimal ConsumedFat { get; set; }

    public decimal ConsumedCarbs { get; set; }

    public decimal RemainingCalories { get; set; }

    public decimal RemainingProtein { get; set; }

    public decimal RemainingFat { get; set; }

    public decimal RemainingCarbs { get; set; }
}