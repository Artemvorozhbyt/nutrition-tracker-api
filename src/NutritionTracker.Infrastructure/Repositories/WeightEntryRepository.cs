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
			.ToListAsync();
	}
}