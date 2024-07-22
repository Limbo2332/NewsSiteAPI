using System.Net;
using System.Net.Http.Json;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSite.BLL.Exceptions;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Endpoints.Abstract;
using Newtonsoft.Json;

namespace NewsSite.IntegrationTests.Systems.Endpoints;

public class NewsEndpointsTests(EndpointsFactory factory) : BaseEndpointsTests(factory)
{
    #region GetNewsByRubricAsync Tests

    [Fact]
    public async Task GetNewsByRubricAsync_ShouldReturnOk()
    {
        // Arrange
        var rubricId = DbContext.NewsRubrics.First().RubricId;

        var body = new PageSettings
        {
            PageSorting = new PageSorting
            {
                SortingProperty = nameof(News.Content),
                SortingOrder = SortingOrder.Ascending
            }
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync($"api/news/by-rubric/{rubricId}", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent =
            await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().BeInAscendingOrder(i => i.Content);
    }

    #endregion

    #region GetNewsByTagsAsync Tests

    [Fact]
    public async Task GetNewsByTagsAsync_ShouldReturnOk()
    {
        // Arrange
        var tagsIds = DbContext.NewsTags.Take(2).Select(t => t.TagId).ToList();

        var body = new NewsByTagsRequest
        {
            TagsIds = tagsIds,
            PageSettings = new PageSettings
            {
                PageSorting = new PageSorting
                {
                    SortingProperty = nameof(News.Subject),
                    SortingOrder = SortingOrder.Descending
                }
            }
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/news/by-tags", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().BeInDescendingOrder(i => i.Subject);
    }

    #endregion

    #region GetNewsByAuthorAsync Tests

    [Fact]
    public async Task GetNewsByAuthorAsync_ShouldReturnOk()
    {
        // Arrange
        var news = DbContext.News.AsNoTracking().First();
        var authorId = news.CreatedBy;
        PageSettings? body = null;

        // Act
        var response = await HttpClient.PostAsJsonAsync($"api/news/by-author/{authorId}", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Select(x => x.AuthorId).Should().Contain(authorId);
    }

    #endregion

    #region GetNewsByIdAsync Tests

    [Fact]
    public async Task GetNewsByIdAsync_ShouldReturnOk()
    {
        // Arrange
        var newsId = DbContext.News.AsNoTracking().First().Id;

        // Act
        var response = await HttpClient.GetAsync($"api/news/{newsId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<NewsResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().Be(newsId);
    }

    #endregion

    #region GetNewsByIdAsync Tests

    [Fact]
    public async Task GetNewsByPeriodOfTimeAsync_ShouldReturnOk()
    {
        // Arrange
        var news = await DbContext.News.AsNoTracking().FirstAsync();
        var body = new NewsByPeriodOfDateRequest
        {
            StartDate = news.UpdatedAt,
            EndDate = new DateTime(news.UpdatedAt.Year + 1, news.UpdatedAt.Month, news.UpdatedAt.Day)
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/news/by-date", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Select(x => x.Id).Should().Contain(news.Id);
    }

    #endregion

    #region DeleteNewsAsync Tests

    [Fact]
    public async Task DeleteNewsAsync_ShouldReturnNoContent()
    {
        // Arrange
        var news = DbContext.News.First();
        var newsToDelete = DbContext.News.Add(new News
        {
            Id = Guid.NewGuid(),
            Subject = news.Subject,
            Content = news.Content,
            CreatedAt = news.CreatedAt,
            CreatedBy = news.CreatedBy,
            UpdatedAt = news.UpdatedAt
        });

        await DbContext.SaveChangesAsync();

        // Act
        var response = await HttpClient.DeleteAsync($"api/news/{newsToDelete.Entity.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        DbContext.News.Select(a => a.Id).Should().NotContain(newsToDelete.Entity.Id);
    }

    #endregion

    #region GetNewsAsync Tests

    [Fact]
    public async Task GetNewsAsync_ShouldReturnOkResult()
    {
        // Arrange
        PageSettings? body = null;

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/news/", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

        responseContent.Should().NotBeNull();
    }

    [Fact]
    public async Task GetNewsAsync_ShouldBeSorted_WhenPageSortingIsApplied()
    {
        // Arrange
        var body = new PageSettings
        {
            PageSorting = new PageSorting
            {
                SortingProperty = nameof(News.Subject),
                SortingOrder = SortingOrder.Descending
            }
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/news", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().BeInDescendingOrder(i => i.Subject);
    }

    [Fact]
    public async Task GetNewsAsync_ShouldBeFiltered_WhenPageFilteringIsApplied()
    {
        // Arrange
        var body = new PageSettings
        {
            PageFiltering = new PageFiltering
            {
                PropertyName = "Subject",
                PropertyValue = "u"
            }
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/news", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().Contain(x => x.Subject.Contains("u"));
    }

    [Fact]
    public async Task GetNewsAsync_ShouldBePaginated_WhenPagePaginationIsApplied()
    {
        // Arrange
        var body = new PageSettings
        {
            PagePagination = new PagePagination
            {
                PageSize = 2,
                PageNumber = 3
            }
        };
        var expectedCount =
            DbContext.News.Count() - body.PagePagination.PageSize * (body.PagePagination.PageNumber - 1);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/news", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Count.Should().Be(expectedCount);
    }

    #endregion

    #region CreateNewsAsync Tests

    [Fact]
    public async Task CreateNewsAsync_ShouldReturnCreated_WhenValidationIsPassed()
    {
        // Arrange
        var author = DbContext.Authors.AsNoTracking().First();
        Authenticate(author);

        var newNewsRequest = new NewNewsRequest
        {
            Content = new Faker().Random.String2(ConfigurationConstants.CONTENT_MAXLENGTH - 1),
            Subject = new Faker().Random.String2(ConfigurationConstants.SUBJECT_MAXLENGTH - 1),
            AuthorId = author.Id
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/news/create", newNewsRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadFromJsonAsync<NewsResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().NotBe(Guid.Empty);
        responseContent.AuthorId.Should().Be(newNewsRequest.AuthorId);
        responseContent.Content.Should().Be(newNewsRequest.Content);
        responseContent.Subject.Should().Be(newNewsRequest.Subject);

        // Cleanup
        var news = await DbContext.News.FindAsync(responseContent.Id);
        DbContext.News.Remove(news);
        await DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateNewsAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
    {
        // Arrange
        var author = DbContext.Authors.AsNoTracking().First();
        Authenticate(author);

        var newNewsRequest = new NewNewsRequest
        {
            Content = new Faker().Random.String2(ConfigurationConstants.CONTENT_MAXLENGTH + 1),
            Subject = new Faker().Random.String2(ConfigurationConstants.SUBJECT_MAXLENGTH + 1),
            AuthorId = author.Id
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/news/create", newNewsRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
        problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateNewsAsync_ShouldReturnNotFoundRequest_WhenAuthorWasNotFound()
    {
        // Arrange
        Authenticate();

        var authorId = Guid.Empty;
        var newNewsRequest = new NewNewsRequest
        {
            Content = new Faker().Random.String2(ConfigurationConstants.CONTENT_MINLENGTH + 1),
            Subject = new Faker().Random.String2(ConfigurationConstants.SUBJECT_MINLENGTH + 1),
            AuthorId = authorId
        };
        var exceptionMessage = new NotFoundException(nameof(Author), newNewsRequest.AuthorId).Message;

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/news/create", newNewsRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(exceptionMessage);
        problemDetails.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    #endregion

    #region UpdateNewsAsync Tests

    [Fact]
    public async Task UpdateNewsAsync_ShouldReturnCreated_WhenValidationIsPassed()
    {
        // Arrange
        var author = DbContext.Authors.AsNoTracking().First();
        Authenticate(author);

        var updateNewsRequest = new UpdateNewsRequest
        {
            Content = new Faker().Random.String2(ConfigurationConstants.CONTENT_MAXLENGTH - 1),
            Subject = new Faker().Random.String2(ConfigurationConstants.SUBJECT_MAXLENGTH - 1),
            AuthorId = author.Id
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/news", updateNewsRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<NewsResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().NotBe(Guid.Empty);
        responseContent.AuthorId.Should().Be(updateNewsRequest.AuthorId);
        responseContent.Content.Should().Be(updateNewsRequest.Content);
        responseContent.Subject.Should().Be(updateNewsRequest.Subject);
    }

    [Fact]
    public async Task UpdateNewsAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
    {
        // Arrange
        var author = DbContext.Authors.AsNoTracking().First();
        Authenticate(author);

        var updateNewsRequest = new UpdateNewsRequest
        {
            Content = new Faker().Random.String2(ConfigurationConstants.CONTENT_MAXLENGTH + 1),
            Subject = new Faker().Random.String2(ConfigurationConstants.SUBJECT_MAXLENGTH + 1),
            AuthorId = author.Id
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/news", updateNewsRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
        problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
    }

    #endregion
}