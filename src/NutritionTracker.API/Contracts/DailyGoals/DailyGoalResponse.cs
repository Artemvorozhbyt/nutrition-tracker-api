namespace NutritionTracker.API.Contracts.DailyGoals;

public class DailyGoalResponse
{
    public decimal Calories { get; set; }

    public decimal Protein { get; set; }

    public decimal Fat { get; set; }

    public decimal Carbs { get; set; }
}