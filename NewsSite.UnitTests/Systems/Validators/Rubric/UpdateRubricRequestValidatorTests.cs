using FluentValidation.TestHelper;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.UI.Validators.Rubric;
using NewsSite.UnitTests.TestData.Validators.Rubric;

namespace NewsSite.UnitTests.Systems.Validators.Rubric;

public class UpdateRubricRequestValidatorTests
{
    private readonly UpdateRubricRequestValidator _sut;

    public UpdateRubricRequestValidatorTests()
    {
        _sut = new UpdateRubricRequestValidator();
    }

    [Theory]
    [ClassData(typeof(RubricRequestValidatorWrongData))]
    public async Task ValidateUpdateRubricRequest_Should_Fail_WhenWrongUpdateRubricRequest(string rubricName)
    {
        // Arrange
        var updateRubricRequest = new UpdateRubricRequest
        {
            Name = rubricName
        };

        // Act
        var result = await _sut.TestValidateAsync(updateRubricRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [ClassData(typeof(RubricRequestValidatorCorrectData))]
    public async Task ValidateUpdateRubricRequest_Should_Success_WhenUpdateRubricRequest(string rubricName)
    {
        // Arrange
        var updateRubricRequest = new UpdateRubricRequest
        {
            Name = rubricName
        };

        // Act
        var result = await _sut.TestValidateAsync(updateRubricRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}