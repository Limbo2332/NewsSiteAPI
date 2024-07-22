using FluentValidation.TestHelper;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.UI.Validators.News;
using NewsSite.UnitTests.TestData.Validators.News;

namespace NewsSite.UnitTests.Systems.Validators.News;

public class NewNewsRequestValidatorTests
{
    private readonly NewNewsRequestValidator _sut;

    public NewNewsRequestValidatorTests()
    {
        _sut = new NewNewsRequestValidator();
    }

    [Theory]
    [ClassData(typeof(NewsRequestValidatorWrongData))]
    public async Task ValidateNewNewsRequest_Should_Fail_WhenWrongNewNewsRequest(string subject, string content)
    {
        // Arrange
        var newNewsRequest = new NewNewsRequest
        {
            Content = content,
            Subject = subject
        };

        // Act
        var result = await _sut.TestValidateAsync(newNewsRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Subject);
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Theory]
    [ClassData(typeof(NewsRequestValidatorCorrectData))]
    public async Task ValidateNewNewsRequest_Should_Success_WhenNewNewsRequest(string subject, string content)
    {
        // Arrange
        var newNewsRequest = new NewNewsRequest
        {
            Content = content,
            Subject = subject
        };

        // Act
        var result = await _sut.TestValidateAsync(newNewsRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Subject);
        result.ShouldNotHaveValidationErrorFor(x => x.Content);
    }
}