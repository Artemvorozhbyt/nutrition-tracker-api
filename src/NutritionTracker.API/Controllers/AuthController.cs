using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using NutritionTracker.API.Contracts.Auth;
using NutritionTracker.Application.Interfaces;
using NutritionTracker.Domain.Entities;

namespace NutritionTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _repository;

    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(
    IUserRepository repository,
    IJwtTokenGenerator jwtTokenGenerator)
    {
        _repository = repository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existingUser =
            await _repository.GetByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return BadRequest("Email already exists");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            Gender = request.Gender,
            Age = request.Age,
            Height = request.Height,
            Weight = request.Weight,
            Goal = request.Goal,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(user);

        return StatusCode(201);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _repository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            return Unauthorized("Invalid email or password");
        }

        var passwordValid =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if (!passwordValid)
        {
            return Unauthorized("Invalid email or password");
        }

        var token =
            _jwtTokenGenerator.GenerateToken(
                user.Id,
                user.Email);

        return Ok(new LoginResponse
        {
            AccessToken = token
        });
    }
}