using Microsoft.EntityFrameworkCore;
using NutritionTracker.Application.Interfaces;
using NutritionTracker.Domain.Entities;
using NutritionTracker.Infrastructure.Persistence;

namespace NutritionTracker.Infrastructure.Repositories;

public class DailyGoalRepository : IDailyGoalRepository
{
    private readonly ApplicationDbContext _context;

    public DailyGoalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DailyGoal?> GetByUserIdAsync(Guid userId)
    {
        return await _context.DailyGoals
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task AddAsync(DailyGoal dailyGoal)
    {
        await _context.DailyGoals.AddAsync(dailyGoal);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DailyGoal dailyGoal)
    {
        _context.DailyGoals.Update(dailyGoal);
        await _context.SaveChangesAsync();
    }
}