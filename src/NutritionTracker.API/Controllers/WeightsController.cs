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

        var userId = Guid.Parse(userIdClaim);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var existingEntry =
            await _repository.GetByUserIdAndDateAsync(
                userId,
                today);

        if (existingEntry is not null)
        {
            existingEntry.Weight = request.Weight;

            await _repository.UpdateAsync(existingEntry);

            return Ok();
        }

        var weightEntry = new WeightEntry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Weight = request.Weight,
            Date = today,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(weightEntry);

        return StatusCode(201);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
    Guid id,
    UpdateWeightEntryRequest request)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);

        var weightEntry =
            await _repository.GetByIdAsync(id);

        if (weightEntry is null)
        {
            return NotFound();
        }

        if (weightEntry.UserId != userId)
        {
            return Forbid();
        }

        weightEntry.Weight = request.Weight;

        await _repository.UpdateAsync(weightEntry);

        return Ok();
    }

    [HttpDelete("{id}")]
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

        var weightEntry =
            await _repository.GetByIdAsync(id);

        if (weightEntry is null)
        {
            return NotFound();
        }

        if (weightEntry.UserId != userId)
        {
            return Forbid();
        }

        await _repository.DeleteAsync(weightEntry);

        return NoContent();
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

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
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

        if (!entries.Any())
        {
            return Ok(new WeightHistoryResponse());
        }

        var ordered =
            entries.OrderBy(x => x.Date).ToList();

        var startWeight = ordered.First().Weight;
        var currentWeight = ordered.Last().Weight;

        var response = new WeightHistoryResponse
        {
            StartWeight = startWeight,
            CurrentWeight = currentWeight,
            Difference = currentWeight - startWeight,

            Entries = ordered
                .Select(x => new WeightEntryResponse
                {
                    Id = x.Id,
                    Weight = x.Weight,
                    Date = x.Date
                })
                .ToList()
        };

        return Ok(response);
    }
}