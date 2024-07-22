using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Exceptions;
using NewsSite.DAL.Constants;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Endpoints.Abstract;
using Newtonsoft.Json;

namespace NewsSite.IntegrationTests.Systems.Endpoints;

public class TagsEndpointsTests(EndpointsFactory factory) : BaseEndpointsTests(factory)
{
    #region AddTagForNewsAsync Tests

    [Fact]
    public async Task AddTagForNewsAsync_ShouldReturnCreated_WhenTagIsAppliedForNews()
    {
        // Arrange
        Authenticate();
        var tag = DbContext.Tags.First();

        var newNewsTagRequest = new NewsTagRequest
        {
            TagId = tag.Id,
            NewsId = DbContext.News.First().Id
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/tags/newsTag", newNewsTagRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadFromJsonAsync<TagResponse>();

        responseContent.Should().NotBeNull();
        responseContent.Should().BeEquivalentTo(tag, src => src.Excluding(t => t.CreatedAt));
    }

    #endregion

    #region DeleteTagAsync Tests

    [Fact]
    public async Task DeleteTagAsync_ShouldReturnNoContent()
    {
        // Arrange
        Authenticate();

        var tagToDelete = await DbContext.Tags.AddAsync(new Tag
        {
            Name = "TagToDelete"
        });

        // Act
        var response = await HttpClient.DeleteAsync($"api/tags/{tagToDelete.Entity.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        DbContext.Tags.Select(t => t.Id).Should().NotContain(tagToDelete.Entity.Id);
    }

    #endregion

    #region DeleteTagForNewsAsync Tests

    [Fact]
    public async Task DeleteTagForNewsAsync_ShouldReturnNoContent()
    {
        // Arrange
        Authenticate();
        var newsTag = DbContext.NewsTags.First();

        // Act
        var response = await HttpClient.DeleteAsync($"api/tags/newsTag/{newsTag.TagId}/{newsTag.NewsId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        DbContext.NewsTags.Should().NotContain(newsTag);
    }

    #endregion

    #region GetTagsAsync Tests

    [Fact]
    public async Task GetTagsAsync_ShouldReturnOkResult()
    {
        // Arrange
        PageSettings? body = null;

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/tags", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<TagResponse>>();

        responseContent.Should().NotBeNull();
    }

    [Fact]
    public async Task GetTagsAsync_ShouldBeSorted_WhenPageSortingIsApplied()
    {
        // Arrange
        var body = new PageSettings
        {
            PageSorting = new PageSorting
            {
                SortingProperty = nameof(Tag.Name),
                SortingOrder = SortingOrder.Descending
            }
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/tags", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<TagResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().BeInDescendingOrder(i => i.Name);
    }

    [Fact]
    public async Task GetTagsAsync_ShouldBeFiltered_WhenPageFilteringIsApplied()
    {
        // Arrange
        var body = new PageSettings
        {
            PageFiltering = new PageFiltering
            {
                PropertyName = nameof(Tag.Name),
                PropertyValue = "u"
            }
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/tags", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<TagResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().Contain(x => x.Name.Contains("u"));
    }

    [Fact]
    public async Task GetTagsAsync_ShouldBePaginated_WhenPagePaginationIsApplied()
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
        var response = await HttpClient.PostAsJsonAsync("api/tags", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<TagResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Count.Should().Be(1);
    }

    #endregion

    #region GetTagByIdAsync Tests

    [Fact]
    public async Task GetTagByIdAsync_ShouldReturnOk()
    {
        // Arrange
        Authenticate();
        var tag = DbContext.Tags.First();

        // Act
        var response = await HttpClient.GetAsync($"api/tags/{tag.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<TagResponse>();

        responseContent.Should().NotBeNull();
        responseContent.Should().BeEquivalentTo(tag, src => src.Excluding(t => t.CreatedAt));
    }

    [Fact]
    public async Task GetTagByIdAsync_ShouldReturnBadRequest_WhenTagNotFound()
    {
        // Arrange
        Authenticate();
        var tagId = Guid.Empty;
        var exceptionMessage = new NotFoundException(nameof(Tag), tagId).Message;

        // Act
        var response = await HttpClient.GetAsync($"api/tags/{tagId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(exceptionMessage);
        problemDetails.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    #endregion

    #region CreateNewTagAsync Tests

    [Fact]
    public async Task CreateNewTagAsync_ShouldReturnCreated_WhenNewTagIsCreated()
    {
        // Arrange
        Authenticate();

        var newTagRequest = new NewTagRequest
        {
            Name = "Name"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/tags/create", newTagRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadFromJsonAsync<TagResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().NotBe(Guid.Empty);
        responseContent.Name.Should().BeEquivalentTo(newTagRequest.Name);

        // Cleanup
        var tag = await DbContext.Tags.FindAsync(responseContent.Id);
        DbContext.Tags.Remove(tag);
        await DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateNewTagAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
    {
        // Arrange
        Authenticate();

        var newTagRequest = new NewTagRequest
        {
            Name = string.Empty
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/tags/create", newTagRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
        problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
    }

    #endregion

    #region UpdateTagAsync Tests

    [Fact]
    public async Task UpdateTagAsync_ShouldReturnOk_WhenTagExists()
    {
        // Arrange
        Authenticate();
        var tag = DbContext.Tags.First();

        var newNewsTagRequest = new UpdateTagRequest
        {
            Id = tag.Id,
            Name = tag.Name + "1"
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/tags", newNewsTagRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<TagResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Name.Should().Be(tag.Name + "1");
    }

    [Fact]
    public async Task UpdateTagAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
    {
        // Arrange
        Authenticate();

        var newTagRequest = new UpdateTagRequest
        {
            Id = DbContext.Tags.First().Id,
            Name = string.Empty
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/tags", newTagRequest);

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