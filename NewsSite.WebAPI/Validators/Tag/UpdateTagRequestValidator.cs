using FluentValidation;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Validators.Tag;

public class UpdateTagRequestValidator : AbstractValidator<UpdateTagRequest>
{
    public UpdateTagRequestValidator()
    {
        RuleFor(nt => nt.Name)
            .CustomTagName();
    }
}