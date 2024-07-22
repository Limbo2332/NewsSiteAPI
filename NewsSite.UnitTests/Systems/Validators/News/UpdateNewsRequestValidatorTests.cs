using FluentValidation.TestHelper;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.UI.Validators.News;
using NewsSite.UnitTests.TestData.Validators.News;

namespace NewsSite.UnitTests.Systems.Validators.News;

public class UpdateNewsRequestValidatorTests
{
    private readonly UpdateNewsRequestValidator _sut;

    public UpdateNewsRequestValidatorTests()
    {
        _sut = new UpdateNewsRequestValidator();
    }

    [Theory]
    [ClassData(typeof(NewsRequestValidatorWrongData))]
    public async Task ValidateUpdateNewsRequest_Should_Fail_WhenWrongUpdateNewsRequest(string subject, string content)
    {
        // Arrange
        var updateNewsRequest = new UpdateNewsRequest
        {
            Content = content,
            Subject = subject
        };

        // Act
        var result = await _sut.TestValidateAsync(updateNewsRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Subject);
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Theory]
    [ClassData(typeof(NewsRequestValidatorCorrectData))]
    public async Task ValidateUpdateNewsRequest_Should_Success_WhenUpdateNewsRequest(string subject, string content)
    {
        // Arrange
        var updateNewsRequest = new UpdateNewsRequest
        {
            Content = content,
            Subject = subject
        };

        // Act
        var result = await _sut.TestValidateAsync(updateNewsRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Subject);
        result.ShouldNotHaveValidationErrorFor(x => x.Content);
    }
}