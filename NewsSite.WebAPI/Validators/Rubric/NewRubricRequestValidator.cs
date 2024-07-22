using FluentValidation;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Validators.Rubric;

public class NewRubricRequestValidator : AbstractValidator<NewRubricRequest>
{
    public NewRubricRequestValidator()
    {
        RuleFor(nr => nr.Name)
            .CustomRubricName();
    }
}