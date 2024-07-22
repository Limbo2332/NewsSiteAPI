using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Filters;

namespace NewsSite.UI.Endpoints;

public static class NewsEndpoints
{
    public static RouteGroupBuilder MapNewsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/news").AllowAnonymous();

        group.MapPost(string.Empty, GetNewsAsync);
        group.MapPost("by-rubric/{rubricId:guid}", GetNewsByRubricAsync);
        group.MapPost("by-tags", GetNewsByTagsAsync);
        group.MapPost("by-author/{authorId:guid}", GetNewsByAuthor);
        group.MapGet("{id:guid}", GetNewsByIdAsync);
        group.MapPost("by-date", GetNewsByPeriodOfTimeAsync);

        group.MapPost("create", CreateNewsAsync).RequireAuthorization()
            .AddEndpointFilter<ValidationExceptionFilter<NewNewsRequest>>();

        group.MapPut(string.Empty, UpdateNewsAsync).RequireAuthorization()
            .AddEndpointFilter<ValidationExceptionFilter<UpdateNewsRequest>>();

        group.MapDelete("{id:guid}", DeleteNewsAsync).RequireAuthorization();

        return group;
    }

    private static async Task<IResult> GetNewsAsync(
        [FromBody] PageSettings? pageSettings,
        INewsService newsService)
    {
        var pageList = await newsService.GetNewsAsync(pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<IResult> GetNewsByRubricAsync(
        [FromRoute] Guid rubricId,
        [FromBody] PageSettings? pageSettings,
        INewsService newsService)
    {
        var pageList = await newsService.GetNewsByRubricAsync(rubricId, pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<IResult> GetNewsByTagsAsync(
        [FromBody] NewsByTagsRequest newsByTagsRequest,
        INewsService newsService)
    {
        var pageList = await newsService.GetNewsByTagsAsync(newsByTagsRequest.TagsIds, newsByTagsRequest.PageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<IResult> GetNewsByAuthor(
        [FromBody] PageSettings? pageSettings,
        [FromRoute] Guid authorId,
        INewsService newsService)
    {
        var pageList = await newsService.GetNewsByAuthorAsync(authorId, pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<IResult> GetNewsByIdAsync(
        [FromRoute] Guid id,
        INewsService newsService)
    {
        var pageList = await newsService.GetNewsByIdAsync(id);

        return Results.Ok(pageList);
    }

    private static async Task<IResult> GetNewsByPeriodOfTimeAsync(
        [FromBody] NewsByPeriodOfDateRequest request,
        INewsService newsService)
    {
        var pageList =
            await newsService.GetNewsByPeriodOfTimeAsync(request.StartDate, request.EndDate, request.PageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<Results<Created<NewsResponse>, ProblemHttpResult>> CreateNewsAsync(
        [FromBody] NewNewsRequest newNewsRequest,
        INewsService newsService)
    {
        var result = await newsService.CreateNewNewsAsync(newNewsRequest);

        return TypedResults.Created(nameof(CreateNewsAsync), result);
    }

    private static async Task<Results<Ok<NewsResponse>, ProblemHttpResult>> UpdateNewsAsync(
        [FromBody] UpdateNewsRequest updateNewsRequest,
        INewsService newsService)
    {
        var result = await newsService.UpdateNewsAsync(updateNewsRequest);

        return TypedResults.Ok(result);
    }

    private static async Task<IResult> DeleteNewsAsync(
        [FromRoute] Guid id,
        INewsService newsService)
    {
        await newsService.DeleteNewsAsync(id);

        return Results.NoContent();
    }
}