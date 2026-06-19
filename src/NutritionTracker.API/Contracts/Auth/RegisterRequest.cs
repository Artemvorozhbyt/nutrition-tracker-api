using NutritionTracker.Domain.Enums;

namespace NutritionTracker.API.Contracts.Auth;

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    public int Age { get; set; }

    public decimal Height { get; set; }

    public decimal Weight { get; set; }

    public GoalType Goal { get; set; }
}