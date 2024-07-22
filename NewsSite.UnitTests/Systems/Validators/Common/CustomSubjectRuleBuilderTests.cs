using FluentValidation;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.UI.Extensions;
using NewsSite.UnitTests.TestData.Validators.Common;

namespace NewsSite.UnitTests.Systems.Validators.Common;

public class CustomSubjectRuleBuilderTests
{
    private readonly InlineValidator<string> _sut;

    public CustomSubjectRuleBuilderTests()
    {
        _sut = new InlineValidator<string>();
        _sut.RuleFor(x => x)
            .CustomSubject();
    }

    [Fact]
    public async Task CustomSubject_Should_Fail_WhenSubjectIsEmpty()
    {
        // Arrange
        var emptySubject = string.Empty;

        // Act
        var result = await _sut.ValidateAsync(emptySubject);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x =>
                x.ErrorMessage == ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.SUBJECT_PROPERTY_NAME));
        }
    }

    [Fact]
    public async Task CustomSubject_Should_Fail_WhenSubjectLessThanMinLength()
    {
        // Arrange
        var subject = new string('s', ConfigurationConstants.SUBJECT_MINLENGTH - 1);

        // Act
        var result = await _sut.ValidateAsync(subject);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages.GetEntityWithWrongMinimumLengthMessage(
                    ValidationMessages.SUBJECT_PROPERTY_NAME,
                    ConfigurationConstants.SUBJECT_MINLENGTH));
        }
    }

    [Fact]
    public async Task CustomSubject_Should_Fail_WhenSubjectMoreThanMaxLength()
    {
        // Arrange
        var subject = new string('s', ConfigurationConstants.SUBJECT_MAXLENGTH + 1);

        // Act
        var result = await _sut.ValidateAsync(subject);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage == ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                    ValidationMessages.SUBJECT_PROPERTY_NAME,
                    ConfigurationConstants.SUBJECT_MAXLENGTH));
        }
    }


    [Theory]
    [ClassData(typeof(NoSpacesRegexFormatWrongData))]
    public async Task CustomSubject_Should_Fail_WhenSubjectNotInCorrectFormat(string subject)
    {
        // Act
        var result = await _sut.ValidateAsync(subject);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(
                x => x.ErrorMessage ==
                     ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.SUBJECT_PROPERTY_NAME));
        }
    }

    [Fact]
    public async Task CustomSubject_Should_Success_WhenCorrectSubject()
    {
        // Arrange
        var subjectValue = new Faker().Random.String2(ConfigurationConstants.SUBJECT_MAXLENGTH - 1);

        // Act
        var result = await _sut.ValidateAsync(subjectValue);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}