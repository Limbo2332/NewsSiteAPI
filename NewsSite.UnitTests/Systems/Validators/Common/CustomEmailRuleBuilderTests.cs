using FluentValidation;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.UI.Extensions;

namespace NewsSite.UnitTests.Systems.Validators.Common;

public class CustomEmailRuleBuilderTests
{
    private readonly InlineValidator<string> _sut;

    public CustomEmailRuleBuilderTests()
    {
        _sut = new InlineValidator<string>();
        _sut.RuleFor(x => x)
            .CustomEmail();
    }

    [Fact]
    public async Task CustomEmail_Should_Fail_WhenEmailIsEmpty()
    {
        // Arrange
        var emptyEmail = string.Empty;

        // Act
        var result = await _sut.ValidateAsync(emptyEmail);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x =>
                x.ErrorMessage == ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.EMAIL_PROPERTY_NAME));
        }
    }

    [Fact]
    public async Task CustomEmail_Should_Fail_WhenEmailLessThanMinLength()
    {
        // Arrange
        var email = new string('s', ConfigurationConstants.EMAIL_MINLENGTH - 1);

        // Act
        var result = await _sut.ValidateAsync(email);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages
                    .GetEntityWithWrongMinimumLengthMessage(
                        ValidationMessages.EMAIL_PROPERTY_NAME,
                        ConfigurationConstants.EMAIL_MINLENGTH));
        }
    }

    [Fact]
    public async Task CustomEmail_Should_Fail_WhenEmailMoreThanMaxLength()
    {
        // Arrange
        var email = new string('s', ConfigurationConstants.EMAIL_MAXLENGTH + 1);

        // Act
        var result = await _sut.ValidateAsync(email);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages
                    .GetEntityWithWrongMaximumLengthMessage(
                        ValidationMessages.EMAIL_PROPERTY_NAME,
                        ConfigurationConstants.EMAIL_MAXLENGTH));
        }
    }

    [Theory]
    [InlineData("example.gmail.com")]
    [InlineData("example@gmailcom")]
    [InlineData("@gmail.com")]
    [InlineData("example@gmail.c")]
    public async Task CustomEmail_Should_Fail_WhenEmailNotInCorrectFormat(string email)
    {
        // Act
        var result = await _sut.ValidateAsync(email);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage ==
                     ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.EMAIL_PROPERTY_NAME));
        }
    }

    [Theory]
    [InlineData("example@gmail.com")]
    [InlineData("example@hotline.com")]
    [InlineData("babay@ukr.net")]
    [InlineData("example@lll.kpi.ua")]
    public async Task CustomEmail_Should_Success_WhenCorrectEmail(string email)
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