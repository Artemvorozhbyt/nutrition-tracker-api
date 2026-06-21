using FluentValidation;
using NutritionTracker.API.Contracts.DailyGoals;

namespace NutritionTracker.API.Validators;

public class UpdateDailyGoalRequestValidator
    : AbstractValidator<UpdateDailyGoalRequest>
{
    public UpdateDailyGoalRequestValidator()
    {
        RuleFor(x => x.Calories)
            .GreaterThan(0);

        RuleFor(x => x.Protein)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Fat)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Carbs)
            .GreaterThanOrEqualTo(0);
    }
}