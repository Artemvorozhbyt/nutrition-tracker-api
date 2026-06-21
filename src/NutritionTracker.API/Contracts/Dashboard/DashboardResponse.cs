namespace NutritionTracker.API.Contracts.Dashboard;

public class DashboardResponse
{
    public decimal? CurrentWeight { get; set; }

    public decimal GoalCalories { get; set; }

    public decimal ConsumedCalories { get; set; }

    public decimal RemainingCalories { get; set; }

    public int MealsToday { get; set; }
}