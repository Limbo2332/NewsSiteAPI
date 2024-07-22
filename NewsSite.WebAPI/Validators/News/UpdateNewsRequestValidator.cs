using FluentValidation;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Validators.News;

public class UpdateNewsRequestValidator : AbstractValidator<UpdateNewsRequest>
{
    public UpdateNewsRequestValidator()
    {
        RuleFor(nn => nn.Subject)
            .CustomSubject();

        RuleFor(nn => nn.Content)
            .CustomContent();
    }
}