using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Filters;

namespace NewsSite.UI.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth");

        group.MapPost(nameof(Login), Login).AddEndpointFilter<ValidationExceptionFilter<UserLoginRequest>>();
        group.MapPost(nameof(Register), Register).AddEndpointFilter<ValidationExceptionFilter<UserRegisterRequest>>();

        return group;
    }

    private static async Task<Results<Ok<LoginUserResponse>, ProblemHttpResult>> Login(
        [FromBody] UserLoginRequest userLoginRequest,
        IAuthService authService)
    {
        var response = await authService.LoginAsync(userLoginRequest);

        return TypedResults.Ok(response);
    }

    private static async Task<Results<Created<NewUserResponse>, ProblemHttpResult>> Register(
        [FromBody] UserRegisterRequest userRegisterRequest,
        IAuthService authService)
    {
        var response = await authService.RegisterAsync(userRegisterRequest);

        return TypedResults.Created(nameof(Register), response);
    }
}