using FluentValidation;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Validators.Auth;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(ur => ur.Email)
            .CustomEmail();

        RuleFor(ur => ur.Password)
            .CustomPassword();
    }
}