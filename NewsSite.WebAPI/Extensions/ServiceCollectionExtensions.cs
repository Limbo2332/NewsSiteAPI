using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.MappingProfiles;
using NewsSite.BLL.Services;
using NewsSite.DAL.Context;
using NewsSite.DAL.Repositories;
using NewsSite.DAL.Repositories.Base;
using NewsSite.UI.Validators.Auth;

namespace NewsSite.UI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<OnlineNewsContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Online news API",
                Version = "v1",
                Description = "Online news API Services."
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                BearerFormat = "JWT",
                Scheme = "Bearer",
                Description = "JWT Authorization header using the Bearer scheme.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    public static void AddAuthenticationWithJwt(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config.GetSection("JWT:Issuer").Value,
                    ValidAudience = config.GetSection("JWT:Audience").Value,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:SecretKey").Value!))
                };
            });

        services.AddAuthorization();
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuthorsRepository, AuthorsRepository>();
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<IRubricsRepository, RubricsRepository>();
        services.AddScoped<ITagsRepository, TagsRepository>();
    }

    public static void RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AuthorsProfile>();
            cfg.AddProfile<NewsProfile>();
            cfg.AddProfile<RubricsProfile>();
            cfg.AddProfile<TagsProfile>();
        }, Assembly.GetAssembly(typeof(AuthorsProfile)));
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthorsService, AuthorsService>();
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<IRubricsService, RubricsService>();
        services.AddScoped<ITagsService, TagsService>();
    }

    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserLoginRequestValidator>();
    }

    public static void AutoMigrateDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<OnlineNewsContext>();

        context.Database.Migrate();
    }
}