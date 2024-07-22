using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSite.BLL.Exceptions;
using NewsSite.DAL.Constants;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Endpoints.Abstract;
using NewsSite.UnitTests.TestData;
using Newtonsoft.Json;

namespace NewsSite.IntegrationTests.Systems.Endpoints;

public class RubricsEndpointsTests(EndpointsFactory factory) : BaseEndpointsTests(factory)
{
    #region AddRubricForNewsAsync Tests

    [Fact]
    public async Task AddRubricForNewsAsync_ShouldReturnCreated_WhenRubricIsAppliedForNews()
    {
        // Arrange
        Authenticate();
        var newsRubric = RepositoriesFakeData.GenerateNewsRubrics(1, 1).First();
        var newNewsRubricRequest = new NewsRubricRequest
        {
            RubricId = newsRubric.RubricId,
            NewsId = newsRubric.NewsId
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/rubrics/newsRubrics", newNewsRubricRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadFromJsonAsync<RubricResponse>();

        responseContent.Should().NotBeNull();
        DbContext.NewsRubrics.AsNoTracking().ToList().Should().ContainEquivalentOf(newsRubric);

        // Cleanup
        DbContext.NewsRubrics.Remove(newsRubric);
        await DbContext.SaveChangesAsync();
    }

    #endregion

    #region DeleteRubricAsync Tests

    [Fact]
    public async Task DeleteRubricAsync_ShouldReturnNoContent()
    {
        // Arrange
        Authenticate();

        var rubricToDelete = await DbContext.Rubrics.AddAsync(new Rubric
        {
            Name = "RubricToDelete"
        });

        // Act
        var response = await HttpClient.DeleteAsync($"api/rubrics/{rubricToDelete.Entity.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        DbContext.Rubrics.Select(r => r.Id).Should().NotContain(rubricToDelete.Entity.Id);
    }

    #endregion

    #region DeleteRubricForNewsAsync Tests

    [Fact]
    public async Task DeleteRubricForNewsAsync_ShouldReturnNoContent()
    {
        // Arrange
        Authenticate();
        var newsRubrics = DbContext.NewsRubrics.First();

        // Act
        var response =
            await HttpClient.DeleteAsync($"api/rubrics/newsRubrics/{newsRubrics.RubricId}/{newsRubrics.NewsId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        DbContext.NewsRubrics.Should().NotContain(newsRubrics);
    }

    #endregion

    #region GetRubricsAsync Tests

    [Fact]
    public async Task GetRubricsAsync_ShouldReturnOkResult()
    {
        // Arrange
        PageSettings? body = null;

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/rubrics", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<RubricResponse>>();

        responseContent.Should().NotBeNull();
    }

    [Fact]
    public async Task GetRubricsAsync_ShouldBeSorted_WhenPageSortingIsApplied()
    {
        // Arrange
        var body = new PageSettings
        {
            PageSorting = new PageSorting
            {
                SortingProperty = nameof(Rubric.Name),
                SortingOrder = SortingOrder.Descending
            }
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/rubrics", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<RubricResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().BeInDescendingOrder(i => i.Name);
    }

    [Fact]
    public async Task GetRubricsAsync_ShouldBeFiltered_WhenPageFilteringIsApplied()
    {
        // Arrange
        Authenticate();
        var body = new PageSettings
        {
            PageFiltering = new PageFiltering
            {
                PropertyName = nameof(Rubric.Name),
                PropertyValue = "u"
            }
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/rubrics", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<RubricResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().Contain(x => x.Name.Contains("u"));
    }

    [Fact]
    public async Task GetRubricsAsync_ShouldBePaginated_WhenPagePaginationIsApplied()
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

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/rubrics", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<RubricResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Count.Should().Be(1);
    }

    #endregion

    #region GetRubricByIdAsync Tests

    [Fact]
    public async Task GetRubricByIdAsync_ShouldReturnOk()
    {
        // Arrange
        Authenticate();
        var rubric = DbContext.Rubrics.First();

        // Act
        var response = await HttpClient.GetAsync($"api/rubrics/{rubric.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<RubricResponse>();

        responseContent.Should().NotBeNull();
        responseContent.Should().BeEquivalentTo(rubric, src => src.Excluding(t => t.CreatedAt));
    }

    [Fact]
    public async Task GetRubricByIdAsync_ShouldReturnBadRequest_WhenRubricNotFound()
    {
        // Arrange
        Authenticate();

        var rubricId = Guid.Empty;
        var exceptionMessage = new NotFoundException(nameof(Rubric), rubricId).Message;

        // Act
        var response = await HttpClient.GetAsync($"api/rubrics/{rubricId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(exceptionMessage);
        problemDetails.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    #endregion

    #region CreateNewRubricAsync Tests

    [Fact]
    public async Task CreateNewRubricAsync_ShouldReturnCreated_WhenNewRubricIsCreated()
    {
        // Arrange
        Authenticate();

        var newRubricRequest = new NewRubricRequest
        {
            Name = "NameNameName"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/rubrics/create", newRubricRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadFromJsonAsync<RubricResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().NotBe(Guid.Empty);
        responseContent.Name.Should().BeEquivalentTo(newRubricRequest.Name);

        // Cleanup
        var rubric = await DbContext.Rubrics.FindAsync(responseContent.Id);
        DbContext.Rubrics.Remove(rubric);
        await DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateNewRubricAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
    {
        // Arrange
        Authenticate();

        var newRubricRequest = new NewRubricRequest
        {
            Name = string.Empty
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/rubrics/create", newRubricRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
        problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
    }

    #endregion

    #region UpdateRubricAsync Tests

    [Fact]
    public async Task UpdateRubricAsync_ShouldReturnOk_WhenRubricExists()
    {
        // Arrange
        Authenticate();
        var rubric = DbContext.Rubrics.First();

        var newNewsRubricRequest = new UpdateRubricRequest
        {
            Id = rubric.Id,
            Name = "NameNameName"
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/rubrics", newNewsRubricRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<RubricResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Name.Should().Be("NameNameName");
    }

    [Fact]
    public async Task UpdateRubricAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
    {
        // Arrange
        Authenticate();

        var newRubricRequest = new UpdateRubricRequest
        {
            Id = DbContext.Rubrics.First().Id,
            Name = string.Empty
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/rubrics", newRubricRequest);

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