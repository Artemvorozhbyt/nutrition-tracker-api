using Microsoft.EntityFrameworkCore;
using NutritionTracker.Application.Interfaces;
using NutritionTracker.Domain.Entities;
using NutritionTracker.Infrastructure.Persistence;

namespace NutritionTracker.Infrastructure.Repositories;

public class MealEntryRepository : IMealEntryRepository
{
    private readonly ApplicationDbContext _context;

    public MealEntryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(MealEntry mealEntry)
    {
        await _context.MealEntries.AddAsync(mealEntry);

        await _context.SaveChangesAsync();
    }

    public async Task<List<MealEntry>> GetByUserIdAsync(Guid userId)
    {
        return await _context.MealEntries
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }
    public async Task<MealEntry?> GetByIdAsync(Guid id)
    {
        return await _context.MealEntries.FindAsync(id);
    }

    public async Task DeleteAsync(MealEntry mealEntry)
    {
        _context.MealEntries.Remove(mealEntry);

        await _context.SaveChangesAsync();
    }
}