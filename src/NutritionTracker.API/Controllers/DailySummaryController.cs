using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionTracker.API.Contracts.DailySummary;
using NutritionTracker.Application.Interfaces;

namespace NutritionTracker.API.Controllers;

[ApiController]
[Route("api/daily-summary")]
[Authorize]
public class DailySummaryController : ControllerBase
{
    private readonly IMealEntryRepository _mealRepository;
    private readonly IDailyGoalRepository _dailyGoalRepository;

    public DailySummaryController(
        IMealEntryRepository mealRepository,
        IDailyGoalRepository dailyGoalRepository)
    {
        _mealRepository = mealRepository;
        _dailyGoalRepository = dailyGoalRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var meals =
            await _mealRepository.GetByUserIdAndDateAsync(
                userId,
                today);

        var goal =
            await _dailyGoalRepository.GetByUserIdAsync(userId);

        if (goal is null)
        {
            return NotFound("Daily goal not found");
        }

        var consumedCalories = meals.Sum(x => x.Calories);
        var consumedProtein = meals.Sum(x => x.Protein);
        var consumedFat = meals.Sum(x => x.Fat);
        var consumedCarbs = meals.Sum(x => x.Carbs);

        var response = new DailySummaryResponse
        {
            GoalCalories = goal.TargetCalories,
            GoalProtein = goal.TargetProtein,
            GoalFat = goal.TargetFat,
            GoalCarbs = goal.TargetCarbs,

            ConsumedCalories = consumedCalories,
            ConsumedProtein = consumedProtein,
            ConsumedFat = consumedFat,
            ConsumedCarbs = consumedCarbs,

            RemainingCalories = goal.TargetCalories - consumedCalories,
            RemainingProtein = goal.TargetProtein - consumedProtein,
            RemainingFat = goal.TargetFat - consumedFat,
            RemainingCarbs = goal.TargetCarbs - consumedCarbs
        };

        return Ok(response);
    }
}