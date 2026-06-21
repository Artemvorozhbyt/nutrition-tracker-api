using NutritionTracker.Domain.Entities;

namespace NutritionTracker.Application.Interfaces;

public interface IWeightEntryRepository
{
    Task AddAsync(WeightEntry weightEntry);

    Task<List<WeightEntry>> GetByUserIdAsync(Guid userId);

    Task<WeightEntry?> GetLatestByUserIdAsync(Guid userId);
}