using FluentValidation;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.UI.Extensions;
using NewsSite.UnitTests.TestData.Validators.Common;

namespace NewsSite.UnitTests.Systems.Validators.Common;

public class CustomTagNameRuleBuilderTests
{
    private readonly InlineValidator<string> _sut;

    public CustomTagNameRuleBuilderTests()
    {
        _sut = new InlineValidator<string>();
        _sut.RuleFor(x => x)
            .CustomTagName();
    }

    [Fact]
    public async Task CustomTagName_Should_Fail_WhenTagNameIsEmpty()
    {
        // Arrange
        var emptyTagName = string.Empty;

        // Act
        var result = await _sut.ValidateAsync(emptyTagName);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x =>
                x.ErrorMessage == ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.NAME_PROPERTY_NAME));
        }
    }

    [Fact]
    public async Task CustomTagName_Should_Fail_WhenTagNameLessThanMinLength()
    {
        // Arrange
        var tagName = new string('s', ConfigurationConstants.TAG_MINLENGTH - 1);

        // Act
        var result = await _sut.ValidateAsync(tagName);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages.GetEntityWithWrongMinimumLengthMessage(
                    ValidationMessages.NAME_PROPERTY_NAME,
                    ConfigurationConstants.TAG_MINLENGTH));
        }
    }

    [Fact]
    public async Task CustomTagName_Should_Fail_WhenTagNameMoreThanMaxLength()
    {
        // Arrange
        var tagName = new string('s', ConfigurationConstants.TAG_MAXLENGTH + 1);

        // Act
        var result = await _sut.ValidateAsync(tagName);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                    ValidationMessages.NAME_PROPERTY_NAME,
                    ConfigurationConstants.TAG_MAXLENGTH));
        }
    }


    [Theory]
    [ClassData(typeof(NoSpacesRegexFormatWrongData))]
    public async Task CustomTagName_Should_Fail_WhenTagNameNotInCorrectFormat(string tagName)
    {
        // Act
        var result = await _sut.ValidateAsync(tagName);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage ==
                     ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.NAME_PROPERTY_NAME));
        }
    }

    [Fact]
    public async Task CustomTagName_Should_Success_WhenCorrectTagName()
    {
        // Arrange
        var tagNameValue = new Faker().Random.String(
            ConfigurationConstants.TAG_MINLENGTH,
            ConfigurationConstants.TAG_MAXLENGTH);

        // Act
        var result = await _sut.ValidateAsync(tagNameValue);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}