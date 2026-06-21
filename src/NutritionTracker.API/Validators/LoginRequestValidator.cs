using FluentValidation;
using NutritionTracker.API.Contracts.Auth;

namespace NutritionTracker.API.Validators;

public class LoginRequestValidator
    : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}