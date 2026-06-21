using FluentValidation.TestHelper;
using NutritionTracker.API.Contracts.Weights;
using NutritionTracker.API.Validators;

namespace NutritionTracker.UnitTests.Validators;

public class CreateWeightEntryRequestValidatorTests
{
    private readonly CreateWeightEntryRequestValidator _validator =
        new();

    [Fact]
    public void Should_Have_Error_When_Weight_Is_Less_Than_Zero()
    {
        var model = new CreateWeightEntryRequest
        {
            Weight = -10
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Weight);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Weight_Is_Valid()
    {
        var model = new CreateWeightEntryRequest
        {
            Weight = 92.4m
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Weight);
    }
}