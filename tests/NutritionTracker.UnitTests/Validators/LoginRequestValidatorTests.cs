using FluentValidation.TestHelper;
using NutritionTracker.API.Contracts.Auth;
using NutritionTracker.API.Validators;

namespace NutritionTracker.UnitTests.Validators;

public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator =
        new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new LoginRequest
        {
            Email = "",
            Password = "123456"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new LoginRequest
        {
            Email = "test@test.com",
            Password = ""
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Request()
    {
        var model = new LoginRequest
        {
            Email = "test@test.com",
            Password = "123456"
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}