using FluentValidation;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Validators.Rubric;

public class UpdateRubricRequestValidator : AbstractValidator<UpdateRubricRequest>
{
    public UpdateRubricRequestValidator()
    {
        RuleFor(nr => nr.Name)
            .CustomRubricName();
    }
}