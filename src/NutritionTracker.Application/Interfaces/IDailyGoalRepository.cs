using NutritionTracker.Domain.Entities;

namespace NutritionTracker.Application.Interfaces;

public interface IDailyGoalRepository
{
    Task<DailyGoal?> GetByUserIdAsync(Guid userId);

    Task AddAsync(DailyGoal dailyGoal);

    Task UpdateAsync(DailyGoal dailyGoal);
}