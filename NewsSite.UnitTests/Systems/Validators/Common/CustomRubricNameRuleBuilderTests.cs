using FluentValidation;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.UI.Extensions;
using NewsSite.UnitTests.TestData.Validators.Common;

namespace NewsSite.UnitTests.Systems.Validators.Common;

public class CustomRubricNameRuleBuilderTests
{
    private readonly InlineValidator<string> _sut;

    public CustomRubricNameRuleBuilderTests()
    {
        _sut = new InlineValidator<string>();
        _sut.RuleFor(x => x)
            .CustomRubricName();
    }

    [Fact]
    public async Task CustomRubricName_Should_Fail_WhenRubricNameIsEmpty()
    {
        // Arrange
        var emptyRubricName = string.Empty;

        // Act
        var result = await _sut.ValidateAsync(emptyRubricName);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x =>
                x.ErrorMessage == ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.NAME_PROPERTY_NAME));
        }
    }

    [Fact]
    public async Task CustomRubricName_Should_Fail_WhenRubricNameLessThanMinLength()
    {
        // Arrange
        var rubricName = new string('s', ConfigurationConstants.RUBRIC_MINLENGTH - 1);

        // Act
        var result = await _sut.ValidateAsync(rubricName);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages.GetEntityWithWrongMinimumLengthMessage(
                    ValidationMessages.NAME_PROPERTY_NAME,
                    ConfigurationConstants.RUBRIC_MINLENGTH));
        }
    }

    [Fact]
    public async Task CustomRubricName_Should_Fail_WhenRubricNameMoreThanMaxLength()
    {
        // Arrange
        var rubricName = new string('s', ConfigurationConstants.RUBRIC_MAXLENGTH + 1);

        // Act
        var result = await _sut.ValidateAsync(rubricName);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                    ValidationMessages.NAME_PROPERTY_NAME,
                    ConfigurationConstants.RUBRIC_MAXLENGTH));
        }
    }


    [Theory]
    [ClassData(typeof(NoSpacesRegexFormatWrongData))]
    public async Task CustomRubricName_Should_Fail_WhenRubricNameNotInCorrectFormat(string rubricName)
    {
        // Act
        var result = await _sut.ValidateAsync(rubricName);

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
    public async Task CustomRubricName_Should_Success_WhenCorrectRubricName()
    {
        // Arrange
        var rubricNameValue = new Faker().Random.String2(
            ConfigurationConstants.RUBRIC_MINLENGTH,
            ConfigurationConstants.RUBRIC_MAXLENGTH);

        // Act
        var result = await _sut.ValidateAsync(rubricNameValue);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}