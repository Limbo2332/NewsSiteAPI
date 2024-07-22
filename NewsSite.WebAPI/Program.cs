using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context;
using NewsSite.DAL.DTO;
using NewsSite.DAL.Interceptors;
using NewsSite.UI.Extensions;

namespace NewsSite.UI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<UpdatedEntityInterceptor>();
        builder.Services.AddDbContext<OnlineNewsContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("NewsDatabaseConnection")));

        builder.Services.AddProblemDetails();

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

        builder.Services.AddIdentity();
        builder.Services.AddAuthenticationWithJwt(builder.Configuration);

        builder.Services.RegisterRepositories();
        builder.Services.RegisterAutoMapper();
        builder.Services.AddCustomServices();

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.RegisterValidators();

        builder.Services.ConfigureSwagger();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.RegisterRouteGroups();

        app.AutoMigrateDatabase();

        app.Run();
    }
}