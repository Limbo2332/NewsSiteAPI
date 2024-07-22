using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSite.BLL.Exceptions;
using NewsSite.DAL.Constants;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Endpoints.Abstract;
using Newtonsoft.Json;

namespace NewsSite.IntegrationTests.Systems.Endpoints;

public class AuthorsEndpointsTests(EndpointsFactory factory) : BaseEndpointsTests(factory)
{
    #region GetAuthorsAsync Tests

    [Fact]
    public async Task GetAuthorsAsync_ShouldReturnOk()
    {
        // Arrange
        var body = new PageSettings
        {
            PageSorting = new PageSorting
            {
                SortingProperty = nameof(Author.FullName),
                SortingOrder = SortingOrder.Descending
            }
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/authors", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<AuthorResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().BeInDescendingOrder(i => i.FullName);
    }

    #endregion

    #region DeleteAuthorAsync Tests

    [Fact]
    public async Task DeleteAuthorAsync_ShouldReturnNoContent()
    {
        // Arrange
        var email = "newEmail@gmail.com";
        var fullName = "newFullName";
        var authorToDelete = await DbContext.Authors.AddAsync(new Author
        {
            Id = Guid.NewGuid(),
            Email = email,
            BirthDate = DateTime.UtcNow.AddYears(-40),
            FullName = fullName,
            IdentityUser = new IdentityUser
            {
                Email = email,
                UserName = fullName,
                NormalizedEmail = email.ToUpper()
            }
        });
        await DbContext.SaveChangesAsync();

        Authenticate(authorToDelete.Entity);

        // Act
        var response = await HttpClient.DeleteAsync($"api/authors/{authorToDelete.Entity.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        DbContext.Authors.Select(a => a.Id).Should().NotContain(authorToDelete.Entity.Id);
    }

    #endregion

    #region GetAuthorById Tests

    [Fact]
    public async Task GetAuthorByIdAsync_ShouldReturnOk_WhenAuthorExists()
    {
        // Arrange
        var authorId = DbContext.Authors.First().Id;

        // Act
        var response = await HttpClient.GetAsync($"api/authors/{authorId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<AuthorResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().Be(authorId);
    }

    [Fact]
    public async Task GetAuthorByIdAsync_ShouldReturnNotFound_WhenAuthorDoesNotExist()
    {
        // Arrange
        var authorId = Guid.Empty;
        var exceptionMessage = new NotFoundException(nameof(Author), authorId).Message;

        // Act
        var response = await HttpClient.GetAsync($"api/authors/{authorId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(exceptionMessage);
        problemDetails.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    #endregion

    #region UpdateAuthorAsync Tests

    [Fact]
    public async Task UpdateAuthorAsync_ShouldReturnOk_WhenValidationIsPassed()
    {
        // Arrange
        var author = DbContext.Authors.AsNoTracking().First();
        Authenticate(author);

        var updatedAuthorRequest = new UpdatedAuthorRequest
        {
            Id = author.Id,
            Email = "testemail@gmail.com",
            FullName = "FullNameFullName",
            BirthDate = new DateTime(1990, 11, 1),
            Sex = !author.Sex,
            PublicInformation = "sdsadas"
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/authors", updatedAuthorRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<AuthorResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().NotBe(Guid.Empty);
        responseContent.Email.Should().Be(updatedAuthorRequest.Email);
        responseContent.FullName.Should().Be(updatedAuthorRequest.FullName);
        responseContent.BirthDate.Should().Be(updatedAuthorRequest.BirthDate);
        responseContent.Sex.Should().Be(updatedAuthorRequest.Sex);
        responseContent.PublicInformation.Should().Be(updatedAuthorRequest.PublicInformation);
    }

    [Fact]
    public async Task UpdateAuthorAsync_ShouldReturnBadRequest_WhenValidationNotPassed()
    {
        // Arrange
        var author = DbContext.Authors.AsNoTracking().First();
        Authenticate(author);

        var updatedAuthorRequest = new UpdatedAuthorRequest
        {
            Id = author.Id,
            Email = string.Empty,
            FullName = string.Empty,
            BirthDate = DateTime.Today.AddYears(1)
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/authors", updatedAuthorRequest);

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