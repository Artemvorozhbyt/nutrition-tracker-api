using NutritionTracker.Domain.Entities;

namespace NutritionTracker.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(Guid id);

    Task AddAsync(Product product);

    Task UpdateAsync(Product product);

    Task DeleteAsync(Product product);

    Task<List<Product>> SearchAsync(string searchTerm);

    Task<List<Product>> GetPagedAsync(
    int page,
    int pageSize);
}