using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionTracker.API.Contracts.Meals;
using NutritionTracker.Application.Interfaces;
using NutritionTracker.Domain.Entities;

namespace NutritionTracker.API.Controllers;

[ApiController]
[Route("api/meals")]
[Authorize]
public class MealsController : ControllerBase
{
    private readonly IMealEntryRepository _mealRepository;
    private readonly IProductRepository _productRepository;

    public MealsController(
        IMealEntryRepository mealRepository,
        IProductRepository productRepository)
    {
        _mealRepository = mealRepository;
        _productRepository = productRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMealEntryRequest request)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);

        var product =
            await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return NotFound("Product not found");
        }

        var factor = request.WeightInGrams / 100m;

        var mealEntry = new MealEntry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductId = product.Id,

            Date = DateOnly.FromDateTime(DateTime.UtcNow),

            WeightInGrams = request.WeightInGrams,

            Calories = product.CaloriesPer100g * factor,
            Protein = product.ProteinPer100g * factor,
            Fat = product.FatPer100g * factor,
            Carbs = product.CarbsPer100g * factor,

            CreatedAt = DateTime.UtcNow
        };

        await _mealRepository.AddAsync(mealEntry);

        return StatusCode(201);
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

        var meals =
            await _mealRepository.GetByUserIdAsync(userId);

        var response = meals
            .Select(x => new MealEntryResponse
            {
                Id = x.Id,
                ProductId = x.ProductId,
                WeightInGrams = x.WeightInGrams,
                Calories = x.Calories,
                Protein = x.Protein,
                Fat = x.Fat,
                Carbs = x.Carbs,
                Date = x.Date
            });

        return Ok(response);
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);

        var meal =
            await _mealRepository.GetByIdAsync(id);

        if (meal is null)
        {
            return NotFound();
        }

        if (meal.UserId != userId)
        {
            return Forbid();
        }

        await _mealRepository.DeleteAsync(meal);

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
    Guid id,
    UpdateMealEntryRequest request)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);

        var meal =
            await _mealRepository.GetByIdAsync(id);

        if (meal is null)
        {
            return NotFound();
        }

        if (meal.UserId != userId)
        {
            return Forbid();
        }

        var product =
            await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return NotFound("Product not found");
        }

        var factor = request.WeightInGrams / 100m;

        meal.ProductId = product.Id;
        meal.WeightInGrams = request.WeightInGrams;

        meal.Calories = product.CaloriesPer100g * factor;
        meal.Protein = product.ProteinPer100g * factor;
        meal.Fat = product.FatPer100g * factor;
        meal.Carbs = product.CarbsPer100g * factor;

        await _mealRepository.UpdateAsync(meal);

        return NoContent();
    }
}