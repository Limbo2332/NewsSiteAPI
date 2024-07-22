using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Endpoints.Abstract;
using Newtonsoft.Json;

namespace NewsSite.IntegrationTests.Systems.Endpoints;

public class AuthEndpointsTests(EndpointsFactory factory) : BaseEndpointsTests(factory)
{
    [Fact]
    public async Task Login_ShouldReturnProblemDetails_WhenValidationIsWrong()
    {
        // Arrange
        var userLoginRequest = new UserLoginRequest
        {
            Email = string.Empty,
            Password = string.Empty
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/auth/login", userLoginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
        problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnNotFound_WhenNoSuchUser()
    {
        // Arrange
        var userLoginRequest = new UserLoginRequest
        {
            Email = "userNotExists@gmail.com",
            Password = "userNotExists1!"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/auth/login", userLoginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Login_ShouldReturnInvalidEmailOrPasswordRequest_WhenValidationIsWrong()
    {
        // Arrange
        var email = "testEmailForTest@gmail.com";
        var testPassword = "12345678Password@";
        var userRegisterRequest = new UserRegisterRequest
        {
            Email = email,
            Password = testPassword,
            FullName = "registerFullNameTwo",
            BirthDate = DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER)
        };
        await HttpClient.PostAsJsonAsync("api/auth/register", userRegisterRequest);

        var userLoginRequest = new UserLoginRequest
        {
            Email = email,
            Password = testPassword + "1"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/auth/login", userLoginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(ValidationMessages.INVALID_EMAIL_OR_PASSWORD_MESSAGE);
        problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnOkResult_WhenUserExists()
    {
        // Arrange
        var email = "email@gmail.com";
        var testPassword = "12345678Password@";
        var userRegisterRequest = new UserRegisterRequest
        {
            Email = email,
            Password = testPassword,
            FullName = "fullNameToRegister",
            BirthDate = DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER)
        };

        await HttpClient.PostAsJsonAsync("api/auth/register", userRegisterRequest);

        var userLoginRequest = new UserLoginRequest
        {
            Email = email,
            Password = testPassword
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/auth/login", userLoginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<LoginUserResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Email.Should().BeEquivalentTo(userLoginRequest.Email);
    }

    [Fact]
    public async Task Register_ShouldReturnProblemDetails_WhenValidationIsWrong()
    {
        // Arrange
        var userRegisterRequest = new UserRegisterRequest
        {
            Email = string.Empty,
            Password = string.Empty
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/auth/register", userRegisterRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
        problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ShouldReturnProblemDetails_WhenTheUser()
    {
        // Arrange
        var userRegisterRequest = new UserRegisterRequest
        {
            Email = string.Empty,
            Password = string.Empty
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/auth/register", userRegisterRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
        problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ShouldReturnCreatedResult()
    {
        // Arrange
        var userRegisterRequest = new UserRegisterRequest
        {
            Email = "registerEmail@gmail.com",
            Password = "registerPassword1!",
            FullName = "registerFullName",
            BirthDate = DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER)
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(
            "api/auth/register",
            userRegisterRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadFromJsonAsync<NewUserResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().NotBe(Guid.Empty);
        responseContent.Email.Should().BeEquivalentTo(userRegisterRequest.Email);
        responseContent.FullName.Should().BeEquivalentTo(userRegisterRequest.FullName);
        responseContent.BirthDate.Should().Be(userRegisterRequest.BirthDate);
    }
}