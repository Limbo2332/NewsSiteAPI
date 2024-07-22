using FluentValidation;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Validators.Tag;

public class NewTagRequestValidator : AbstractValidator<NewTagRequest>
{
    public NewTagRequestValidator()
    {
        RuleFor(nt => nt.Name)
            .CustomTagName();
    }
}