using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Repositories.Base;
using NewsSite.UnitTests.Systems.Services.Abstract;
using NewsSite.UnitTests.TestData;
using NewsSite.UnitTests.TestData.PageSettings.Authors;

namespace NewsSite.UnitTests.Systems.Services;

public class AuthServiceTests : BaseServiceTests
{
    private readonly Mock<IAuthorsRepository> _authorsRepositoryMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly IAuthService _sut;

    public AuthServiceTests()
    {
        _authorsRepositoryMock = new Mock<IAuthorsRepository>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        _sut = new AuthService(
            _userManagerMock.Object,
            _mapper,
            _jwtTokenGeneratorMock.Object,
            _authorsRepositoryMock.Object);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowException_WhenNoIdentityUser()
    {
        // Arrange
        var exceptionMessage = new NotFoundException(nameof(Author)).Message;
        var userLogin = new UserLoginRequest
        {
            Email = "email"
        };

        // Act
        var action = async () => await _sut.LoginAsync(userLogin);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(exceptionMessage);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowException_WhenNoUserWithEmail()
    {
        // Arrange
        var exceptionMessage = new NotFoundException(nameof(Author)).Message;
        var userLogin = new UserLoginRequest
        {
            Email = "email"
        };

        var identityUser = new IdentityUser();

        _userManagerMock
            .Setup(um => um.FindByEmailAsync(userLogin.Email))
            .ReturnsAsync(identityUser);

        _userManagerMock
            .Setup(um => um.CheckPasswordAsync(identityUser, userLogin.Password))
            .ReturnsAsync(true);

        // Act
        var action = async () => await _sut.LoginAsync(userLogin);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(exceptionMessage);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowException_WhenNoCorrectEmailOrPassword()
    {
        // Arrange
        var exceptionMessage = new InvalidEmailOrPasswordException().Message;
        var userLogin = new UserLoginRequest
        {
            Email = "email",
            Password = "password"
        };

        var identityUser = new IdentityUser();

        _userManagerMock
            .Setup(um => um.FindByEmailAsync(userLogin.Email))
            .ReturnsAsync(identityUser);

        _userManagerMock
            .Setup(um => um.CheckPasswordAsync(identityUser, userLogin.Password))
            .ReturnsAsync(false);

        // Act
        var action = async () => await _sut.LoginAsync(userLogin);

        // Assert
        await action.Should()
            .ThrowAsync<InvalidEmailOrPasswordException>()
            .WithMessage(exceptionMessage);
    }

    [Theory]
    [ClassData(typeof(AuthorsTestData))]
    public async Task LoginAsync_ShouldReturnLoginUserResponse_WhenLoginSuccessful(Author author)
    {
        // Arrange
        var userLogin = new UserLoginRequest
        {
            Email = author.Email,
            Password = $"{author.Email} {author.FullName}"
        };

        _userManagerMock
            .Setup(um => um.FindByEmailAsync(userLogin.Email))
            .ReturnsAsync(author.IdentityUser);

        _userManagerMock
            .Setup(um => um.CheckPasswordAsync(author.IdentityUser, userLogin.Password))
            .ReturnsAsync(true);

        _authorsRepositoryMock
            .Setup(ar => ar.GetAuthorByEmailAsync(author.Email))
            .ReturnsAsync(author);

        var loginUserResponse = _mapper.Map<LoginUserResponse>(author);

        // Act
        var result = await _sut.LoginAsync(userLogin);

        // Assert
        using (new AssertionScope())
        {
            result.Email.Should().Be(loginUserResponse.Email);
            result.FullName.Should().Be(loginUserResponse.FullName);
            result.Id.Should().Be(loginUserResponse.Id);
        }
    }

    [Fact]
    public async Task LoginAsync_ShouldGenerateJWTToken_WhenLoginSuccessful()
    {
        // Arrange
        var token = "token";
        var author = new Author
        {
            Email = "email",
            FullName = "FullName",
            IdentityUser = new IdentityUser()
        };

        var userLogin = new UserLoginRequest
        {
            Email = author.Email,
            Password = $"{author.Email} {author.FullName}"
        };

        _userManagerMock
            .Setup(um => um.FindByEmailAsync(userLogin.Email))
            .ReturnsAsync(author.IdentityUser);

        _userManagerMock
            .Setup(um => um.CheckPasswordAsync(author.IdentityUser, userLogin.Password))
            .ReturnsAsync(true);

        _authorsRepositoryMock
            .Setup(ar => ar.GetAuthorByEmailAsync(author.Email))
            .ReturnsAsync(author);

        _jwtTokenGeneratorMock
            .Setup(x => x.GenerateToken(It.IsAny<Author>()))
            .Returns(token);

        // Act
        var result = await _sut.LoginAsync(userLogin);

        // Assert
        result.Token.Should().Be(token);
        _jwtTokenGeneratorMock
            .Verify(x => x.GenerateToken(It.Is<Author>(a =>
                a.Email == author.Email
                && a.FullName == author.FullName)), Times.Once());
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowException_WhenNoCorrectEmailOrPassword()
    {
        // Arrange
        var userRegister = new UserRegisterRequest
        {
            Email = "email",
            FullName = "fullName"
        };

        var errors = new List<IdentityError>
        {
            new()
            {
                Description = "Error1",
                Code = "1"
            },
            new()
            {
                Description = "Error2",
                Code = "2"
            }
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

        var errorMessage = string.Join(' ', errors.Select(e => e.Description));

        // Act
        var action = async () => await _sut.RegisterAsync(userRegister);

        // Assert
        await action.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(errorMessage);
    }

    [Fact]
    public async Task RegisterAsync_ShouldRegisterUser_WhenRegisterSuccessful()
    {
        // Arrange
        var userRegister = new UserRegisterRequest
        {
            Email = "email",
            Password = "password",
            BirthDate = DateTime.UtcNow,
            FullName = "fullName"
        };

        var authors = RepositoriesFakeData.Authors.ToList();

        var authorToAdd = new Author
        {
            BirthDate = userRegister.BirthDate,
            Email = userRegister.Email,
            FullName = userRegister.FullName
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _authorsRepositoryMock
            .Setup(ar => ar.AddAsync(It.IsAny<Author>()))
            .Callback(() => authors.Add(authorToAdd));

        var newUserResponse = _mapper.Map<NewUserResponse>(authorToAdd);

        // Act
        var result = await _sut.RegisterAsync(userRegister);

        // Assert
        result.Should().BeEquivalentTo(newUserResponse, opt => opt.Excluding(r => r.Token));
        authors.Should().ContainEquivalentOf(authorToAdd);
    }
}