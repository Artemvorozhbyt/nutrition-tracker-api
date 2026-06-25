using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionTracker.API.Contracts.Dashboard;
using NutritionTracker.Application.Interfaces;

namespace NutritionTracker.API.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IWeightEntryRepository _weightRepository;
    private readonly IDailyGoalRepository _goalRepository;
    private readonly IMealEntryRepository _mealRepository;

    public DashboardController(
        IWeightEntryRepository weightRepository,
        IDailyGoalRepository goalRepository,
        IMealEntryRepository mealRepository)
    {
        _weightRepository = weightRepository;
        _goalRepository = goalRepository;
        _mealRepository = mealRepository;
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

        var latestWeight =
            await _weightRepository.GetLatestByUserIdAsync(userId);

        var goal =
            await _goalRepository.GetByUserIdAsync(userId);

        if (goal is null)
        {
            return NotFound("Daily goal not found");
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var meals =
            await _mealRepository.GetByUserIdAndDateAsync(
                userId,
                today);

        var consumedProtein =
            meals.Sum(x => x.Protein);

        var consumedFat =
            meals.Sum(x => x.Fat);

        var consumedCarbs =
            meals.Sum(x => x.Carbs);

        var consumedCalories =
            meals.Sum(x => x.Calories);

        var weightEntries =
            await _weightRepository.GetByUserIdAsync(userId);

        decimal? weightDifference = null;

        if (weightEntries.Count >= 2)
        {
            var oldest =
                weightEntries.Last().Weight;

            var newest =
                weightEntries.First().Weight;

            weightDifference =
                newest - oldest;
        }

        var response = new DashboardResponse
        {
            CurrentWeight = latestWeight?.Weight,

            WeightDifference = weightDifference,

            GoalCalories = goal.TargetCalories,

            ConsumedCalories = consumedCalories,

            RemainingCalories =
        goal.TargetCalories - consumedCalories,

            ConsumedProtein = consumedProtein,

            ConsumedFat = consumedFat,

            ConsumedCarbs = consumedCarbs,

            MealsToday = meals.Count
        };

        return Ok(response);
    }
}