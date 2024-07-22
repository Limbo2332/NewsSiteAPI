using System.Net;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using NewsSite.DAL.Constants;

namespace NewsSite.UI.Extensions;

public static class FluentValidationExtensions
{
    public static ProblemDetails ToProblemDetails(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors.Select(x => x.ErrorMessage).Distinct().ToList();

        return new ProblemDetails
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = ValidationMessages.VALIDATION_MESSAGE_RESPONSE,
            Extensions = new Dictionary<string, object?>
            {
                { "validation errors", errors }
            }
        };
    }
}