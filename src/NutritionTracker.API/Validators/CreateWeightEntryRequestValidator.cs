using FluentValidation;
using NutritionTracker.API.Contracts.Weights;

namespace NutritionTracker.API.Validators;

public class CreateWeightEntryRequestValidator
    : AbstractValidator<CreateWeightEntryRequest>
{
    public CreateWeightEntryRequestValidator()
    {
        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .LessThan(500);
    }
}