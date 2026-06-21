using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionTracker.API.Contracts.Weights;
using NutritionTracker.Application.Interfaces;
using NutritionTracker.Domain.Entities;

namespace NutritionTracker.API.Controllers;

[ApiController]
[Route("api/weights")]
[Authorize]
public class WeightsController : ControllerBase
{
    private readonly IWeightEntryRepository _repository;

    public WeightsController(
        IWeightEntryRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateWeightEntryRequest request)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized();
        }

        var weightEntry = new WeightEntry
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userIdClaim),
            Weight = request.Weight,
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(weightEntry);

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

        var entries =
            await _repository.GetByUserIdAsync(
                Guid.Parse(userIdClaim));

        var response = entries
            .Select(x => new WeightEntryResponse
            {
                Id = x.Id,
                Weight = x.Weight,
                Date = x.Date
            });

        return Ok(response);
    }
}