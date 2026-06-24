using Microsoft.AspNetCore.Mvc;
using NutritionTracker.Application.Interfaces;
using NutritionTracker.Domain.Entities;

namespace NutritionTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] string? search,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            return Ok(
                await _repository.SearchAsync(search));
        }

        return Ok(
            await _repository.GetPagedAsync(
                page,
                pageSize));
    }
}