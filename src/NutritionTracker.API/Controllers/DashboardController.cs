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

        var consumedCalories =
            meals.Sum(x => x.Calories);

        var response = new DashboardResponse
        {
            CurrentWeight = latestWeight?.Weight,

            GoalCalories = goal.TargetCalories,

            ConsumedCalories = consumedCalories,

            RemainingCalories =
                goal.TargetCalories - consumedCalories,

            MealsToday = meals.Count
        };

        return Ok(response);
    }
}