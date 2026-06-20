using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionTracker.API.Contracts.DailyGoals;
using NutritionTracker.Application.Interfaces;
using NutritionTracker.Domain.Entities;

namespace NutritionTracker.API.Controllers;

[ApiController]
[Route("api/daily-goals")]
[Authorize]
public class DailyGoalsController : ControllerBase
{
    private readonly IDailyGoalRepository _repository;

    public DailyGoalsController(IDailyGoalRepository repository)
    {
        _repository = repository;
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

        var dailyGoal =
            await _repository.GetByUserIdAsync(userId);

        if (dailyGoal is null)
        {
            return NotFound();
        }

        return Ok(new DailyGoalResponse
        {
            Calories = dailyGoal.TargetCalories,
            Protein = dailyGoal.TargetProtein,
            Fat = dailyGoal.TargetFat,
            Carbs = dailyGoal.TargetCarbs
        });
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        UpdateDailyGoalRequest request)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);

        var dailyGoal =
            await _repository.GetByUserIdAsync(userId);

        if (dailyGoal is null)
        {
            dailyGoal = new DailyGoal
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TargetCalories = request.Calories,
                TargetProtein = request.Protein,
                TargetFat = request.Fat,
                TargetCarbs = request.Carbs,
                CalculatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(dailyGoal);
        }
        else
        {
            dailyGoal.TargetCalories = request.Calories;
            dailyGoal.TargetProtein = request.Protein;
            dailyGoal.TargetFat = request.Fat;
            dailyGoal.TargetCarbs = request.Carbs;
            dailyGoal.CalculatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(dailyGoal);
        }

        return NoContent();
    }
}