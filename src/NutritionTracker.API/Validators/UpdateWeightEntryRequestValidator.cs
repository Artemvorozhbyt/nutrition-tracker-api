using FluentValidation;
using NutritionTracker.API.Contracts.Weights;

namespace NutritionTracker.API.Validators;

public class UpdateWeightEntryRequestValidator
    : AbstractValidator<UpdateWeightEntryRequest>
{
    public UpdateWeightEntryRequestValidator()
    {
        RuleFor(x => x.Weight)
            .GreaterThan(20)
            .LessThan(500);
    }
}