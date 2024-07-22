using FluentValidation.TestHelper;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.UI.Validators.Rubric;
using NewsSite.UnitTests.TestData.Validators.Rubric;

namespace NewsSite.UnitTests.Systems.Validators.Rubric;

public class NewRubricRequestValidatorTests
{
    private readonly NewRubricRequestValidator _sut;

    public NewRubricRequestValidatorTests()
    {
        _sut = new NewRubricRequestValidator();
    }

    [Theory]
    [ClassData(typeof(RubricRequestValidatorWrongData))]
    public async Task ValidateNewRubricRequest_Should_Fail_WhenWrongNewRubricRequest(string rubricName)
    {
        // Arrange
        var newRubricRequest = new NewRubricRequest
        {
            Name = rubricName
        };

        // Act
        var result = await _sut.TestValidateAsync(newRubricRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [ClassData(typeof(RubricRequestValidatorCorrectData))]
    public async Task ValidateNewRubricRequest_Should_Success_WhenNewRubricRequest(string rubricName)
    {
        // Arrange
        var newRubricRequest = new NewRubricRequest
        {
            Name = rubricName
        };

        // Act
        var result = await _sut.TestValidateAsync(newRubricRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}