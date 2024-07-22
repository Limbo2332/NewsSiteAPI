using FluentValidation;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.UI.Extensions;

namespace NewsSite.UnitTests.Systems.Validators.Common;

public class CustomPasswordRuleBuilderTests
{
    private readonly InlineValidator<string> _sut;

    public CustomPasswordRuleBuilderTests()
    {
        _sut = new InlineValidator<string>();
        _sut.RuleFor(x => x)
            .CustomPassword();
    }

    [Fact]
    public async Task CustomPassword_Should_Fail_WhenPasswordIsEmpty()
    {
        // Arrange
        var emptyPassword = string.Empty;

        // Act
        var result = await _sut.ValidateAsync(emptyPassword);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x =>
                x.ErrorMessage ==
                ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.PASSWORD_PROPERTY_NAME));
        }
    }

    [Fact]
    public async Task CustomPassword_Should_Fail_WhenPasswordLessThanMinLength()
    {
        // Arrange
        var password = new string('s', ConfigurationConstants.PASSWORD_MINLENGTH - 1);

        // Act
        var result = await _sut.ValidateAsync(password);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages
                    .GetEntityWithWrongMinimumLengthMessage(
                        ValidationMessages.PASSWORD_PROPERTY_NAME,
                        ConfigurationConstants.PASSWORD_MINLENGTH));
        }
    }

    [Fact]
    public async Task CustomPassword_Should_Fail_WhenPasswordMoreThanMaxLength()
    {
        // Arrange
        var password = new string('s', ConfigurationConstants.PASSWORD_MAXLENGTH + 1);

        // Act
        var result = await _sut.ValidateAsync(password);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages
                    .GetEntityWithWrongMaximumLengthMessage(
                        ValidationMessages.PASSWORD_PROPERTY_NAME,
                        ConfigurationConstants.PASSWORD_MAXLENGTH));
        }
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("12345678aaa")]
    [InlineData("12345678aaaAAA")]
    [InlineData("12345678AAA")]
    [InlineData("12345678$AAA")]
    [InlineData("$AAA")]
    public async Task CustomPassword_Should_Fail_WhenPasswordNotInCorrectFormat(string password)
    {
        // Act
        var result = await _sut.ValidateAsync(password);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages.PASSWORD_WITH_WRONG_FORMAT_MESSAGE);
        }
    }

    [Theory]
    [InlineData("12345678aA!")]
    [InlineData("12345678Aaaa!!!")]
    [InlineData("12345678blfsaF$")]
    [InlineData("12345AAAf*")]
    public async Task CustomPassword_Should_Success_WhenCorrectPassword(string email)
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