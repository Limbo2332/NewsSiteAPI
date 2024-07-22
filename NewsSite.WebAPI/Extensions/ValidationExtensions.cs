using FluentValidation;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;

namespace NewsSite.UI.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> CustomEmail<T>(this IRuleBuilder<T, string> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage(ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.EMAIL_PROPERTY_NAME))
            .EmailAddress()
            .WithMessage(ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.EMAIL_PROPERTY_NAME))
            .Matches(Regexes.NO_SPACES_REGEX)
            .WithMessage(ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.EMAIL_PROPERTY_NAME))
            .Matches(Regexes.EMAIL_REGEX)
            .WithMessage(ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.EMAIL_PROPERTY_NAME))
            .MinimumLength(ConfigurationConstants.EMAIL_MINLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMinimumLengthMessage(
                ValidationMessages.EMAIL_PROPERTY_NAME,
                ConfigurationConstants.EMAIL_MINLENGTH))
            .MaximumLength(ConfigurationConstants.EMAIL_MAXLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                ValidationMessages.EMAIL_PROPERTY_NAME,
                ConfigurationConstants.EMAIL_MAXLENGTH));
    }

    public static IRuleBuilderOptions<T, string> CustomPassword<T>(this IRuleBuilder<T, string> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage(ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.PASSWORD_PROPERTY_NAME))
            .Matches(Regexes.NO_SPACES_REGEX)
            .WithMessage(ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.PASSWORD_PROPERTY_NAME))
            .Matches(Regexes.PASSWORD_REGEX)
            .WithMessage(ValidationMessages.PASSWORD_WITH_WRONG_FORMAT_MESSAGE)
            .MinimumLength(ConfigurationConstants.PASSWORD_MINLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMinimumLengthMessage(
                ValidationMessages.PASSWORD_PROPERTY_NAME,
                ConfigurationConstants.PASSWORD_MINLENGTH))
            .MaximumLength(ConfigurationConstants.PASSWORD_MAXLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                ValidationMessages.PASSWORD_PROPERTY_NAME,
                ConfigurationConstants.PASSWORD_MAXLENGTH));
    }

    public static IRuleBuilderOptions<T, string> CustomFullName<T>(this IRuleBuilder<T, string> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage(ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.FULL_NAME_PROPERTY_NAME))
            .Matches(Regexes.NO_SPACES_REGEX)
            .WithMessage(ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.FULL_NAME_PROPERTY_NAME))
            .Matches(Regexes.FULL_NAME_REGEX)
            .WithMessage(ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.FULL_NAME_PROPERTY_NAME))
            .MinimumLength(ConfigurationConstants.FULL_NAME_MINLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMinimumLengthMessage(
                ValidationMessages.FULL_NAME_PROPERTY_NAME,
                ConfigurationConstants.FULL_NAME_MINLENGTH))
            .MaximumLength(ConfigurationConstants.FULL_NAME_MAXLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                ValidationMessages.FULL_NAME_PROPERTY_NAME,
                ConfigurationConstants.FULL_NAME_MAXLENGTH));
    }

    public static IRuleBuilderOptions<T, string> CustomSubject<T>(this IRuleBuilder<T, string> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage(ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.SUBJECT_PROPERTY_NAME))
            .Matches(Regexes.NO_SPACES_REGEX)
            .WithMessage(ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.SUBJECT_PROPERTY_NAME))
            .MinimumLength(ConfigurationConstants.SUBJECT_MINLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMinimumLengthMessage(
                ValidationMessages.SUBJECT_PROPERTY_NAME,
                ConfigurationConstants.SUBJECT_MINLENGTH))
            .MaximumLength(ConfigurationConstants.SUBJECT_MAXLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                ValidationMessages.SUBJECT_PROPERTY_NAME,
                ConfigurationConstants.SUBJECT_MAXLENGTH));
    }

    public static IRuleBuilderOptions<T, string> CustomContent<T>(this IRuleBuilder<T, string> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage(ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.CONTENT_PROPERTY_NAME))
            .Matches(Regexes.NO_SPACES_REGEX)
            .WithMessage(ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.CONTENT_PROPERTY_NAME))
            .MinimumLength(ConfigurationConstants.CONTENT_MINLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMinimumLengthMessage(
                ValidationMessages.CONTENT_PROPERTY_NAME,
                ConfigurationConstants.CONTENT_MINLENGTH))
            .MaximumLength(ConfigurationConstants.CONTENT_MAXLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                ValidationMessages.CONTENT_PROPERTY_NAME,
                ConfigurationConstants.CONTENT_MAXLENGTH));
    }

    public static IRuleBuilderOptions<T, string> CustomRubricName<T>(this IRuleBuilder<T, string> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage(ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.NAME_PROPERTY_NAME))
            .Matches(Regexes.NO_SPACES_REGEX)
            .WithMessage(ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.NAME_PROPERTY_NAME))
            .MinimumLength(ConfigurationConstants.RUBRIC_MINLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMinimumLengthMessage(
                ValidationMessages.NAME_PROPERTY_NAME,
                ConfigurationConstants.RUBRIC_MINLENGTH))
            .MaximumLength(ConfigurationConstants.RUBRIC_MAXLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                ValidationMessages.NAME_PROPERTY_NAME,
                ConfigurationConstants.RUBRIC_MAXLENGTH));
    }

    public static IRuleBuilderOptions<T, string> CustomTagName<T>(this IRuleBuilder<T, string> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage(ValidationMessages.GetEntityIsEmptyMessage(ValidationMessages.NAME_PROPERTY_NAME))
            .Matches(Regexes.NO_SPACES_REGEX)
            .WithMessage(ValidationMessages.GetEntityWithWrongFormatMessage(ValidationMessages.NAME_PROPERTY_NAME))
            .MinimumLength(ConfigurationConstants.TAG_MINLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMinimumLengthMessage(
                ValidationMessages.NAME_PROPERTY_NAME,
                ConfigurationConstants.TAG_MINLENGTH))
            .MaximumLength(ConfigurationConstants.TAG_MAXLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                ValidationMessages.NAME_PROPERTY_NAME,
                ConfigurationConstants.TAG_MAXLENGTH));
    }
}