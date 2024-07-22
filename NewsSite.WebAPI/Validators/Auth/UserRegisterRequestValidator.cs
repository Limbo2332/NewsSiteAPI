using FluentValidation;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Validators.Auth;

public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterRequestValidator(IAuthorsService authorsService)
    {
        RuleFor(ur => ur.Email)
            .CustomEmail()
            .Must(authorsService.IsEmailUnique)
            .WithMessage(ValidationMessages.GetEntityIsNotUniqueMessage(ValidationMessages.EMAIL_PROPERTY_NAME));

        RuleFor(ur => ur.FullName)
            .CustomFullName()
            .Must(authorsService.IsFullNameUnique)
            .WithMessage(ValidationMessages.GetEntityIsNotUniqueMessage(ValidationMessages.FULL_NAME_PROPERTY_NAME));

        RuleFor(ur => ur.Password)
            .CustomPassword();

        RuleFor(ur => ur.BirthDate)
            .LessThan(DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER))
            .WithMessage(ValidationMessages.BirthDateLessThanYears);

        RuleFor(ur => ur.PublicInformation)
            .MaximumLength(ConfigurationConstants.PUBLIC_INFORMATION_MAXLENGTH)
            .WithMessage(ValidationMessages.GetEntityWithWrongMaximumLengthMessage(
                ValidationMessages.PUBLIC_INFORMATION_PROPERTY_NAME,
                ConfigurationConstants.PUBLIC_INFORMATION_MAXLENGTH));
    }
}