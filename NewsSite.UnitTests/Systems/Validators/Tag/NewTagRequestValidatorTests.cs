using FluentValidation.TestHelper;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.UI.Validators.Tag;
using NewsSite.UnitTests.TestData.Validators.Tag;

namespace NewsSite.UnitTests.Systems.Validators.Tag;

public class NewTagRequestValidatorTests
{
    private readonly NewTagRequestValidator _sut;

    public NewTagRequestValidatorTests()
    {
        _sut = new NewTagRequestValidator();
    }

    [Theory]
    [ClassData(typeof(TagRequestValidatorWrongData))]
    public async Task ValidateNewTagRequest_Should_Fail_WhenWrongNewTagRequest(string rubricName)
    {
        // Arrange
        var newTagRequest = new NewTagRequest
        {
            Name = rubricName
        };

        // Act
        var result = await _sut.TestValidateAsync(newTagRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [ClassData(typeof(TagRequestValidatorCorrectData))]
    public async Task ValidateNewTagRequest_Should_Success_WhenNewTagRequest(string rubricName)
    {
        // Arrange
        var newTagRequest = new NewTagRequest
        {
            Name = rubricName
        };

        // Act
        var result = await _sut.TestValidateAsync(newTagRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}