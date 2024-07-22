using FluentValidation.TestHelper;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.UI.Validators.Tag;
using NewsSite.UnitTests.TestData.Validators.Tag;

namespace NewsSite.UnitTests.Systems.Validators.Tag;

public class UpdateTagRequestValidatorTests
{
    private readonly UpdateTagRequestValidator _sut;

    public UpdateTagRequestValidatorTests()
    {
        _sut = new UpdateTagRequestValidator();
    }

    [Theory]
    [ClassData(typeof(TagRequestValidatorWrongData))]
    public async Task ValidateUpdateTagRequest_Should_Fail_WhenWrongUpdateTagRequest(string rubricName)
    {
        // Arrange
        var newTagRequest = new UpdateTagRequest
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
    public async Task ValidateUpdateTagRequest_Should_Success_WhenUpdateTagRequest(string rubricName)
    {
        // Arrange
        var newTagRequest = new UpdateTagRequest
        {
            Name = rubricName
        };

        // Act
        var result = await _sut.TestValidateAsync(newTagRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}