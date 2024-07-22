using FluentValidation;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.UI.Extensions;
using NewsSite.UnitTests.TestData.Validators.Common;

namespace NewsSite.UnitTests.Systems.Validators.Common;

public class CustomFullNameRuleBuilderTests
{
    private readonly InlineValidator<string> _sut;

    public CustomFullNameRuleBuilderTests()
    {
        _sut = new InlineValidator<string>();
        _sut.RuleFor(x => x)
            .CustomFullName();
    }

    [Fact]
    public async Task CustomFullName_Should_Fail_WhenFullNameIsEmpty()
    {
        // Arrange
        var emptyFullName = string.Empty;

        // Act
        var result = await _sut.ValidateAsync(emptyFullName);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x =>
                x.ErrorMessage ==
                ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.FULL_NAME_PROPERTY_NAME));
        }
    }

    [Fact]
    public async Task CustomFullName_Should_Fail_WhenFullNameLessThanMinLength()
    {
        // Arrange
        var fullName = new string('s', ConfigurationConstants.FULL_NAME_MINLENGTH - 1);

        // Act
        var result = await _sut.ValidateAsync(fullName);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages
                    .GetEntityWithWrongMinimumLengthMessage(
                        ValidationMessages.FULL_NAME_PROPERTY_NAME,
                        ConfigurationConstants.FULL_NAME_MINLENGTH));
        }
    }

    [Fact]
    public async Task CustomFullName_Should_Fail_WhenFullNameMoreThanMaxLength()
    {
        // Arrange
        var fullName = new string('s', ConfigurationConstants.FULL_NAME_MAXLENGTH + 1);

        // Act
        var result = await _sut.ValidateAsync(fullName);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages
                    .GetEntityWithWrongMaximumLengthMessage(
                        ValidationMessages.FULL_NAME_PROPERTY_NAME,
                        ConfigurationConstants.FULL_NAME_MAXLENGTH));
        }
    }

    [Theory]
    [ClassData(typeof(NoSpacesRegexFormatWrongData))]
    public async Task CustomFullName_Should_Fail_WhenFullNameNotInCorrectFormat(string fullName)
    {
        // Act
        var result = await _sut.ValidateAsync(fullName);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage ==
                     ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.FULL_NAME_PROPERTY_NAME));
        }
    }

    [Theory]
    [InlineData("myFullName")]
    [InlineData("customFullName")]
    [InlineData("Easy FullName")]
    public async Task CustomFullName_Should_Success_WhenCorrectFullName(string email)
    {
        // Act
        var result = await _sut.ValidateAsync(email);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}