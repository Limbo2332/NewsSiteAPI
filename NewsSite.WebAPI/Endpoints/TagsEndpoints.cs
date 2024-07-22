using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Filters;

namespace NewsSite.UI.Endpoints;

public static class TagsEndpoints
{
    public static RouteGroupBuilder MapTagsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/tags").RequireAuthorization();

        group.MapPost(string.Empty, GetTagsAsync).AllowAnonymous();
        group.MapGet("{id:guid}", GetTagByIdAsync);
        group.MapPost("create", CreateNewTagAsync).AddEndpointFilter<ValidationExceptionFilter<NewTagRequest>>();
        group.MapPost("newsTag", AddTagForNewsAsync).AddEndpointFilter<ValidationExceptionFilter<NewTagRequest>>();
        group.MapPut(string.Empty, UpdateTagAsync).AddEndpointFilter<ValidationExceptionFilter<UpdateTagRequest>>();
        group.MapDelete("{id:guid}", DeleteTagAsync);
        group.MapDelete("newsTag/{tagId:guid}/{newsId:guid}", DeleteTagForNewsAsync);

        return group;
    }

    private static async Task<IResult> GetTagsAsync(
        [FromBody] PageSettings? pageSettings,
        ITagsService tagsService)
    {
        var pageList = await tagsService.GetTagsAsync(pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<Results<Ok<TagResponse>, ProblemHttpResult>> GetTagByIdAsync(
        [FromRoute] Guid id,
        ITagsService tagsService)
    {
        var tag = await tagsService.GetTagByIdAsync(id);

        return TypedResults.Ok(tag);
    }

    private static async Task<Results<Created<TagResponse>, ProblemHttpResult>> CreateNewTagAsync(
        [FromBody] NewTagRequest newTagRequest,
        ITagsService tagsService)
    {
        var response = await tagsService.CreateNewTagAsync(newTagRequest);

        return TypedResults.Created(nameof(CreateNewTagAsync), response);
    }

    private static async Task<Results<Created<TagResponse>, ProblemHttpResult>> AddTagForNewsAsync(
        [FromBody] NewsTagRequest newsTagRequest,
        ITagsService tagsService)
    {
        var response = await tagsService.AddTagForNewsIdAsync(newsTagRequest);

        return TypedResults.Created(nameof(CreateNewTagAsync), response);
    }

    private static async Task<Results<Ok<TagResponse>, ProblemHttpResult>> UpdateTagAsync(
        [FromBody] UpdateTagRequest updateTagRequest,
        ITagsService tagsService)
    {
        var response = await tagsService.UpdateTagAsync(updateTagRequest);

        return TypedResults.Ok(response);
    }

    private static async Task<NoContent> DeleteTagAsync(
        [FromRoute] Guid id,
        ITagsService tagsService)
    {
        await tagsService.DeleteTagAsync(id);

        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteTagForNewsAsync(
        [FromRoute] Guid tagId,
        [FromRoute] Guid newsId,
        ITagsService tagsService)
    {
        await tagsService.DeleteTagForNewsIdAsync(tagId, newsId);

        return TypedResults.NoContent();
    }
}