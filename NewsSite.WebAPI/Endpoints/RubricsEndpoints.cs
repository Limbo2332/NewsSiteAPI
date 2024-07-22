using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Filters;

namespace NewsSite.UI.Endpoints;

public static class RubricsEndpoints
{
    public static RouteGroupBuilder MapRubricsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/rubrics").RequireAuthorization();
        group.MapPost(string.Empty, GetRubricsAsync).AllowAnonymous();
        group.MapGet("{id:guid}", GetRubricByIdAsync);
        group.MapPost("create", CreateNewRubricAsync).AddEndpointFilter<ValidationExceptionFilter<NewRubricRequest>>();
        group.MapPost("newsRubrics", AddRubricForNewsAsync)
            .AddEndpointFilter<ValidationExceptionFilter<NewRubricRequest>>();
        group.MapPut(string.Empty, UpdateRubricAsync)
            .AddEndpointFilter<ValidationExceptionFilter<UpdateRubricRequest>>();
        group.MapDelete("{id:guid}", DeleteRubricAsync);
        group.MapDelete("newsRubrics/{rubricId:guid}/{newsId:guid}", DeleteRubricForNewsAsync);

        return group;
    }

    private static async Task<IResult> GetRubricsAsync(
        [FromBody] PageSettings? pageSettings,
        IRubricsService rubricsService)
    {
        var pageList = await rubricsService.GetRubricsAsync(pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<Results<Ok<RubricResponse>, ProblemHttpResult>> GetRubricByIdAsync(
        [FromRoute] Guid id,
        IRubricsService rubricsService)
    {
        var rubric = await rubricsService.GetRubricByIdAsync(id);

        return TypedResults.Ok(rubric);
    }

    private static async Task<Results<Created<RubricResponse>, ProblemHttpResult>> CreateNewRubricAsync(
        [FromBody] NewRubricRequest newRubricRequest,
        IRubricsService rubricsService)
    {
        var response = await rubricsService.CreateNewRubricAsync(newRubricRequest);

        return TypedResults.Created(nameof(CreateNewRubricAsync), response);
    }

    private static async Task<Results<Created<RubricResponse>, ProblemHttpResult>> AddRubricForNewsAsync(
        [FromBody] NewsRubricRequest newRubricRequest,
        IRubricsService rubricsService)
    {
        var response = await rubricsService.AddRubricForNewsIdAsync(newRubricRequest);

        return TypedResults.Created(nameof(CreateNewRubricAsync), response);
    }

    private static async Task<Results<Ok<RubricResponse>, ProblemHttpResult>> UpdateRubricAsync(
        [FromBody] UpdateRubricRequest updateRubricRequest,
        IRubricsService rubricsService)
    {
        var response = await rubricsService.UpdateRubricAsync(updateRubricRequest);

        return TypedResults.Ok(response);
    }

    private static async Task<NoContent> DeleteRubricAsync(
        [FromRoute] Guid id,
        IRubricsService rubricsService)
    {
        await rubricsService.DeleteRubricAsync(id);

        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteRubricForNewsAsync(
        [FromRoute] Guid rubricId,
        [FromRoute] Guid newsId,
        IRubricsService rubricsService)
    {
        await rubricsService.DeleteRubricForNewsIdAsync(rubricId, newsId);

        return TypedResults.NoContent();
    }
}