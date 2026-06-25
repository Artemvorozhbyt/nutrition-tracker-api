namespace NutritionTracker.API.Contracts.Dashboard;

public class DashboardResponse
{
    public decimal? CurrentWeight { get; set; }

    public decimal? WeightDifference { get; set; }

    public decimal GoalCalories { get; set; }

    public decimal ConsumedCalories { get; set; }

    public decimal RemainingCalories { get; set; }

    public decimal ConsumedProtein { get; set; }

    public decimal ConsumedFat { get; set; }

    public decimal ConsumedCarbs { get; set; }

    public int MealsToday { get; set; }
}