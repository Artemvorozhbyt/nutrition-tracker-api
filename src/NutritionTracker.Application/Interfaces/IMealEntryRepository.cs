using NutritionTracker.Domain.Entities;

namespace NutritionTracker.Application.Interfaces;

public interface IMealEntryRepository
{
    Task AddAsync(MealEntry mealEntry);

    Task<List<MealEntry>> GetByUserIdAsync(Guid userId);

    Task<MealEntry?> GetByIdAsync(Guid id);

    Task DeleteAsync(MealEntry mealEntry);

    Task<List<MealEntry>> GetByUserIdAndDateAsync(
    Guid userId,
    DateOnly date);
}
