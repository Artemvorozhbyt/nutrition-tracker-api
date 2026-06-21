using FluentValidation.TestHelper;
using NutritionTracker.API.Contracts.Auth;
using NutritionTracker.API.Validators;

namespace NutritionTracker.UnitTests.Validators;

public class RegisterRequestValidatorTests
{
    private readonly RegisterRequestValidator _validator =
        new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new RegisterRequest
        {
            Email = "abc"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var model = new RegisterRequest
        {
            Password = "123"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Request()
    {
        var model = new RegisterRequest
        {
            Email = "test@test.com",
            Password = "password123",
            FirstName = "Artem",
            Age = 25,
            Height = 180,
            Weight = 80
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}