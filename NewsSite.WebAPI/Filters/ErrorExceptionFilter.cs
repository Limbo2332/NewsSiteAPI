using NewsSite.UI.Extensions;

namespace NewsSite.UI.Filters;

public class ErrorExceptionFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.ToProblemDetails());
        }
    }
}