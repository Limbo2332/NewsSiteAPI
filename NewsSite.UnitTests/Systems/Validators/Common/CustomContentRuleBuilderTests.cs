using FluentValidation;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.UI.Extensions;
using NewsSite.UnitTests.TestData.Validators.Common;

namespace NewsSite.UnitTests.Systems.Validators.Common;

public class CustomContentRuleBuilderTests
{
    private readonly InlineValidator<string> _sut;

    public CustomContentRuleBuilderTests()
    {
        _sut = new InlineValidator<string>();
        _sut.RuleFor(x => x)
            .CustomContent();
    }

    [Fact]
    public async Task CustomContent_Should_Fail_WhenContentIsEmpty()
    {
        // Arrange
        var emptyContent = string.Empty;

        // Act
        var result = await _sut.ValidateAsync(emptyContent);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x =>
                x.ErrorMessage == ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.CONTENT_PROPERTY_NAME));
        }
    }

    [Fact]
    public async Task CustomContent_Should_Fail_WhenContentLessThanMinLength()
    {
        // Arrange
        var content = new string('s', ConfigurationConstants.CONTENT_MINLENGTH - 1);

        // Act
        var result = await _sut.ValidateAsync(content);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages.GetEntityWithWrongMinimumLengthMessage(
                    ValidationMessages.CONTENT_PROPERTY_NAME,
                    ConfigurationConstants.CONTENT_MINLENGTH));
        }
    }

    [Fact]
    public async Task CustomContent_Should_Fail_WhenContentMoreThanMaxLength()
    {
        // Arrange
        var content = new string('s', ConfigurationConstants.CONTENT_MAXLENGTH + 1);

        // Act
        var result = await _sut.ValidateAsync(content);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                    ValidationMessages.CONTENT_PROPERTY_NAME,
                    ConfigurationConstants.CONTENT_MAXLENGTH));
        }
    }


    [Theory]
    [ClassData(typeof(NoSpacesRegexFormatWrongData))]
    public async Task CustomContent_Should_Fail_WhenContentNotInCorrectFormat(string content)
    {
        // Act
        var result = await _sut.ValidateAsync(content);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage ==
                     ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.CONTENT_PROPERTY_NAME));
        }
    }

    [Fact]
    public async Task CustomContent_Should_Success_WhenCorrectContent()
    {
        // Arrange
        var contentValue = new Faker().Random.String2(ConfigurationConstants.CONTENT_MAXLENGTH - 1);

        // Act
        var result = await _sut.ValidateAsync(contentValue);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}