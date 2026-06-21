using FluentValidation;
using NutritionTracker.API.Contracts.Meals;

namespace NutritionTracker.API.Validators;

public class CreateMealEntryRequestValidator
	: AbstractValidator<CreateMealEntryRequest>
{
	public CreateMealEntryRequestValidator()
	{
		RuleFor(x => x.ProductId)
			.NotEqual(Guid.Empty);

		RuleFor(x => x.WeightInGrams)
			.GreaterThan(0);
	}
}