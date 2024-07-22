using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Filters;

namespace NewsSite.UI.Endpoints;

public static class AuthorsEndpoints
{
    public static RouteGroupBuilder MapAuthorsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/authors");

        group.MapPost(string.Empty, GetAuthorsAsync).AllowAnonymous();
        group.MapGet("{id:guid}", GetAuthorByIdAsync).AllowAnonymous();

        group.MapPut(string.Empty, UpdateAuthorAsync).RequireAuthorization()
            .AddEndpointFilter<ValidationExceptionFilter<UpdatedAuthorRequest>>();

        group.MapDelete("{id:guid}", DeleteAuthorAsync).RequireAuthorization();

        return group;
    }

    private static async Task<IResult> GetAuthorsAsync(
        [FromBody] PageSettings? pageSettings,
        IAuthorsService authorsService)
    {
        var pageList = await authorsService.GetAuthorsAsync(pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<Results<Ok<AuthorResponse>, ProblemHttpResult>> GetAuthorByIdAsync(
        [FromRoute] Guid id,
        IAuthorsService authorsService)
    {
        var author = await authorsService.GetAuthorByIdAsync(id);

        return TypedResults.Ok(author);
    }

    private static async Task<Results<Ok<AuthorResponse>, ProblemHttpResult>> UpdateAuthorAsync(
        [FromBody] UpdatedAuthorRequest updatedAuthor,
        IAuthorsService authorsService)
    {
        var result = await authorsService.UpdateAuthorAsync(updatedAuthor);

        return TypedResults.Ok(result);
    }

    private static async Task<IResult> DeleteAuthorAsync(
        [FromRoute] Guid id,
        IAuthorsService authorsService)
    {
        await authorsService.DeleteAuthorAsync(id);

        return Results.NoContent();
    }
}