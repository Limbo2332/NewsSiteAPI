using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.Entities;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.UnitTests.TestData;

namespace NewsSite.IntegrationTests.Systems.Endpoints.Abstract;

public class BaseEndpointsTests : IClassFixture<EndpointsFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly OnlineNewsContext DbContext;
    protected readonly HttpClient HttpClient;

    protected readonly Author TestAuthor = new()
    {
        Email = "testEmail@gmail.com",
        FullName = "FullName"
    };

    protected BaseEndpointsTests(EndpointsFactory factory)
    {
        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider.GetRequiredService<OnlineNewsContext>();
        HttpClient = factory.CreateClient();

        if (!DbContext.Authors.Any())
        {
            DbContext.Authors.AddRange(RepositoriesFakeData.Authors.ToList());
            DbContext.News.AddRange(RepositoriesFakeData.News.ToList());
            DbContext.Rubrics.AddRange(RepositoriesFakeData.Rubrics.ToList());
            DbContext.Tags.AddRange(RepositoriesFakeData.Tags.ToList());
            DbContext.NewsTags.AddRange(RepositoriesFakeData.NewsTags.ToList());
            DbContext.NewsRubrics.AddRange(RepositoriesFakeData.NewsRubrics.ToList());

            DbContext.SaveChanges();
        }
    }

    public void Dispose()
    {
        _scope.Dispose();
        DbContext.Dispose();
        HttpClient.Dispose();
    }

    protected void Authenticate(Author? author = null)
    {
        HttpClient.DefaultRequestHeaders.Authorization ??=
            new AuthenticationHeaderValue("Bearer", GenerateToken(author));
    }

    private string GenerateToken(Author? author = null)
    {
        var tokenGenerator = _scope.ServiceProvider.GetRequiredService<IJwtTokenGenerator>();

        return tokenGenerator.GenerateToken(author ?? TestAuthor);
    }
}