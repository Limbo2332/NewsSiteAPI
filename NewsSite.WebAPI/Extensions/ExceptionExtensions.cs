using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Exceptions;

namespace NewsSite.UI.Extensions;

public static class ExceptionExtensions
{
    private static HttpStatusCode ParseException(this Exception exception)
    {
        return exception switch
        {
            BadRequestException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            InvalidEmailOrPasswordException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
    }

    public static ProblemDetails ToProblemDetails(this Exception exception)
    {
        var httpStatusCode = exception.ParseException();

        return new ProblemDetails
        {
            Title = exception.Message,
            Status = (int)httpStatusCode
        };
    }
}