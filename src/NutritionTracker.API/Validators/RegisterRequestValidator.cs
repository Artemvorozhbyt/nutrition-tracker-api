using FluentValidation;
using NutritionTracker.API.Contracts.Auth;

namespace NutritionTracker.API.Validators;

public class RegisterRequestValidator
    : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Age)
            .InclusiveBetween(1, 120);

        RuleFor(x => x.Height)
            .GreaterThan(0);

        RuleFor(x => x.Weight)
            .GreaterThan(0);
    }
}