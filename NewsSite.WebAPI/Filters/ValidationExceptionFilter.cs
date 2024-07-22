using FluentValidation;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Filters;

public class ValidationExceptionFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();

        var entity = context.Arguments
            .OfType<T>()
            .FirstOrDefault(a => a?.GetType() == typeof(T));

        if (entity is null) return await next(context);

        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid) return TypedResults.Problem(validationResult.ToProblemDetails());

        return await next(context);
    }
}