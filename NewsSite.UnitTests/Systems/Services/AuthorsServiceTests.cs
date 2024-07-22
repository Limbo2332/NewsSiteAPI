using System.Globalization;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Extensions;
using NewsSite.BLL.Services;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Repositories.Base;
using NewsSite.UnitTests.Systems.Services.Abstract;
using NewsSite.UnitTests.TestData;
using NewsSite.UnitTests.TestData.PageSettings.Authors;

namespace NewsSite.UnitTests.Systems.Services;

public class AuthorsServiceTests : BaseEntityServiceTests<Author, AuthorResponse>
{
    private readonly Mock<IAuthorsRepository> _authorsRepositoryMock;

    public AuthorsServiceTests()
    {
        _authorsRepositoryMock = new Mock<IAuthorsRepository>();
        QueryableMock = RepositoriesFakeData.Authors.BuildMock();

        Entities = RepositoriesFakeData.Authors;

        _authorsRepositoryMock
            .Setup(ar => ar.GetAll())
            .Returns(QueryableMock);

        Sut = new AuthorsService(
            _userManagerMock.Object,
            _mapper,
            _authorsRepositoryMock.Object);
    }

    protected override IQueryable<Author> QueryableMock { get; }

    protected override AuthorsService Sut { get; }

    protected override List<Author> Entities { get; }

    [Fact]
    public async Task GetAuthorsAsync_ShouldReturnPagedList_WhenPageFilteringBirthDate()
    {
        var propertyName = nameof(Author.BirthDate);
        var propertyValue = RepositoriesFakeData.Authors.First().BirthDate.ToString(CultureInfo.InvariantCulture);

        // Arrange
        var pageSettings = new PageSettings
        {
            PageFiltering = new PageFiltering
            {
                PropertyName = propertyName,
                PropertyValue = propertyValue
            }
        };
        var authorsResponse =
            _mapper.Map<List<AuthorResponse>>(
                QueryableMock
                    .Where(a => propertyValue.IsDateTime()
                                && a.BirthDate >= Convert.ToDateTime(propertyValue, CultureInfo.InvariantCulture)));

        // Act
        var result = await Sut.GetAuthorsAsync(pageSettings);

        // Assert
        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(authorsResponse);
            result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
            result.TotalCount.Should().Be(2);
            result.PageSize.Should().Be(PageList<AuthorResponse>.DefaultPageSize);
            result.PageNumber.Should().Be(1);
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
        }
    }

    [Theory]
    [ClassData(typeof(AuthorsFilteringData))]
    public override Task GetAllAsync_ShouldReturnPagedList_WhenPageFiltering(string propertyName, string propertyValue)
    {
        return base.GetAllAsync_ShouldReturnPagedList_WhenPageFiltering(propertyName, propertyValue);
    }

    [Theory]
    [ClassData(typeof(AuthorsSortingData))]
    public override Task GetAllAsync_ShouldReturnPagedList_WhenPageSortingProperty(SortingOrder order,
        string sortingProperty)
    {
        return base.GetAllAsync_ShouldReturnPagedList_WhenPageSortingProperty(order, sortingProperty);
    }

    [Fact]
    public async Task GetAuthorByIdAsync_ShouldThrowException_WhenNoAuthor()
    {
        // Arrange
        var authorId = Guid.Empty;
        var exceptionMessage = new NotFoundException(nameof(Author), authorId).Message;

        // Act
        var action = async () => await Sut.GetAuthorByIdAsync(authorId);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(exceptionMessage);
    }

    [Theory]
    [ClassData(typeof(AuthorsTestData))]
    public async Task GetAuthorByIdAsync_ShouldReturnAuthor_WhenAuthorExists(Author author)
    {
        // Arrange
        _authorsRepositoryMock
            .Setup(ar => ar.GetByIdAsync(author.Id))
            .ReturnsAsync(author);

        var authorResponse = _mapper.Map<AuthorResponse>(author);

        // Act
        var result = await Sut.GetAuthorByIdAsync(author.Id);

        // Assert
        result.Should().Be(authorResponse);
    }

    [Theory]
    [ClassData(typeof(AuthorsTestData))]
    public async Task UpdateAuthorAsync_ShouldReturnAuthor_WhenAuthorExists(Author author)
    {
        // Arrange
        var updatedAuthor = new UpdatedAuthorRequest
        {
            BirthDate = author.BirthDate,
            Email = author.Email,
            FullName = $"{author.FullName} {author.Email}",
            PublicInformation = author.PublicInformation,
            Sex = true,
            Id = author.Id
        };

        AuthorResponse? authorResponse = null;

        _userManagerMock
            .Setup(um => um.FindByEmailAsync(author.Email).Result)
            .Returns(author.IdentityUser);

        var newAuthor = _mapper.Map<Author>(updatedAuthor);

        _authorsRepositoryMock
            .Setup(ar => ar.GetByIdAsync(author.Id))
            .ReturnsAsync(author);

        _authorsRepositoryMock
            .Setup(ar => ar.UpdateAsync(It.IsAny<Author>()))
            .Callback(() => authorResponse = _mapper.Map<AuthorResponse>(newAuthor));

        _userManagerMock
            .Setup(um => um.SetEmailAsync(newAuthor.IdentityUser, newAuthor.Email))
            .Callback(() => newAuthor.IdentityUser.Email = newAuthor.Email);

        _userManagerMock
            .Setup(um => um.SetUserNameAsync(newAuthor.IdentityUser, newAuthor.FullName))
            .Callback(() => newAuthor.IdentityUser.UserName = newAuthor.FullName);

        // Act
        var result = await Sut.UpdateAuthorAsync(updatedAuthor);

        // Assert
        using (new AssertionScope())
        {
            result.Should().Be(authorResponse);
            result.Email.Should().Be(newAuthor.IdentityUser.Email);
            result.FullName.Should().Be(newAuthor.IdentityUser.UserName);
        }
    }

    [Theory]
    [ClassData(typeof(AuthorsTestData))]
    public async Task DeleteAuthorAsync_ShouldDeleteAuthor_WhenAuthorExists(Author author)
    {
        // Arrange
        var authors = RepositoriesFakeData.Authors.ToList();

        _authorsRepositoryMock
            .Setup(ar => ar.DeleteAsync(author.Id))
            .Callback(() => authors.Remove(author));

        // Act
        await Sut.DeleteAuthorAsync(author.Id);

        // Assert
        authors.Should().NotContain(author);
    }

    [Fact]
    public void IsEmailUnique_ShouldReturnFalse_WhenEmailExists()
    {
        // Arrange
        _authorsRepositoryMock
            .Setup(ar => ar.IsEmailUnique(It.IsAny<string>()))
            .Returns(false);

        // Act
        var result = Sut.IsEmailUnique(It.IsAny<string>());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsEmailUnique_ShouldReturnTrue_WhenEmailExists()
    {
        // Arrange
        _authorsRepositoryMock
            .Setup(ar => ar.IsEmailUnique(It.IsAny<string>()))
            .Returns(true);

        // Act
        var result = Sut.IsEmailUnique(It.IsAny<string>());

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsFullNameUnique_ShouldReturnFalse_WhenEmailExists()
    {
        // Arrange
        _authorsRepositoryMock
            .Setup(ar => ar.IsFullNameUnique(It.IsAny<string>()))
            .Returns(false);

        // Act
        var result = Sut.IsFullNameUnique(It.IsAny<string>());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsFullNameUnique_ShouldReturnTrue_WhenEmailExists()
    {
        // Arrange
        _authorsRepositoryMock
            .Setup(ar => ar.IsFullNameUnique(It.IsAny<string>()))
            .Returns(true);

        // Act
        var result = Sut.IsFullNameUnique(It.IsAny<string>());

        // Assert
        result.Should().BeTrue();
    }
}