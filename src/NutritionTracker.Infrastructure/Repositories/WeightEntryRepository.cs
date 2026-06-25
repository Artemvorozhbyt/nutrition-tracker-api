using Microsoft.EntityFrameworkCore;
using NutritionTracker.Application.Interfaces;
using NutritionTracker.Domain.Entities;
using NutritionTracker.Infrastructure.Persistence;

namespace NutritionTracker.Infrastructure.Repositories;

public class WeightEntryRepository : IWeightEntryRepository
{
	private readonly ApplicationDbContext _context;

	public WeightEntryRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task AddAsync(WeightEntry weightEntry)
	{
		await _context.WeightEntries.AddAsync(weightEntry);

		await _context.SaveChangesAsync();
	}

    public async Task<List<WeightEntry>> GetByUserIdAsync(Guid userId)
    {
        return await _context.WeightEntries
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<WeightEntry?> GetLatestByUserIdAsync(Guid userId)
	{
		return await _context.WeightEntries
			.Where(x => x.UserId == userId)
			.OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
	}

    public async Task<WeightEntry?> GetByUserIdAndDateAsync(
     Guid userId,
     DateOnly date)
    {
        return await _context.WeightEntries
            .Where(x =>
                x.UserId == userId &&
                x.Date == date)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(
        WeightEntry weightEntry)
    {
        _context.WeightEntries.Update(weightEntry);

        await _context.SaveChangesAsync();
    }

    public async Task<WeightEntry?> GetByIdAsync(Guid id)
    {
        return await _context.WeightEntries
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task DeleteAsync(
    WeightEntry weightEntry)
    {
        _context.WeightEntries.Remove(weightEntry);

        await _context.SaveChangesAsync();
    }
}