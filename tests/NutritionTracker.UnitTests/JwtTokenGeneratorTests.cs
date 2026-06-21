using Microsoft.Extensions.Configuration;
using NutritionTracker.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;

namespace NutritionTracker.UnitTests;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void GenerateToken_Should_Return_Token()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Key"] =
                "super-secret-key-for-testing-123456789",
            ["Jwt:Issuer"] =
                "NutritionTracker",
            ["Jwt:Audience"] =
                "NutritionTrackerUsers"
        };

        IConfiguration configuration =
            new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

        var generator =
            new JwtTokenGenerator(configuration);

        // Act
        var token = generator.GenerateToken(
            Guid.NewGuid(),
            "test@test.com");

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));
    }
    [Fact]
    public void GenerateToken_Should_Contain_Email_Claim()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "super-secret-key-for-testing-123456789",
            ["Jwt:Issuer"] = "NutritionTracker",
            ["Jwt:Audience"] = "NutritionTrackerUsers"
        };

        IConfiguration configuration =
            new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

        var generator =
            new JwtTokenGenerator(configuration);

        // Act
        var token = generator.GenerateToken(
            Guid.NewGuid(),
            "test@test.com");

        var jwt =
            new JwtSecurityTokenHandler()
                .ReadJwtToken(token);

        // Assert
        Assert.Contains(
            jwt.Claims,
            x => x.Type == "email"
              && x.Value == "test@test.com");
    }
}