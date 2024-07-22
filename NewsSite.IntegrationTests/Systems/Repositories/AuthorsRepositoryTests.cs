/*
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories;
using NewsSite.DAL.Repositories.Base;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.UnitTests.TestData;
using NewsSite.UnitTests.TestData.PageSettings.Authors;

namespace NewsSite.IntegrationTests.Systems.Repositories
{
    [Collection(nameof(WebFactoryFixture))]
    public class AuthorsRepositoryTests
    {
        private readonly OnlineNewsContext _dbContext;
        private readonly IAuthorsRepository _sut;

        public AuthorsRepositoryTests(WebFactoryFixture fixture)
        {
            _dbContext = fixture.DbContext;
            UserManager<IdentityUser> userManager = fixture.UserManager;

            _sut = new AuthorsRepository(
                _dbContext,
                userManager);
        }

        [Fact]
        public void GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var authors = _dbContext.Authors.AsNoTracking().AsQueryable();

            // Act
            var result = _sut.GetAll();

            // Assert
            result.Should().BeEquivalentTo(authors);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNoAuthor()
        {
            // Arrange
            var authorId = Guid.Empty;

            // Act
            var result = await _sut.GetByIdAsync(authorId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAuthor_WhenAuthorExists()
        {
            // Arrange
            var author = _dbContext.Authors.AsNoTracking().First();

            // Act
            var result = await _sut.GetByIdAsync(author.Id);

            // Assert
            result.Should().BeEquivalentTo(author, opt => opt.Excluding(a => a.IdentityUser));
        }

        [Fact]
        public async Task AddAsync_ShouldAddAuthor_WhenAuthorExists()
        {
            // Arrange
            var author = RepositoriesFakeData.GenerateAuthors(1, 1).First();

            // Act
            await _sut.AddAsync(author);

            // Assert
            var authors = _sut.GetAll();
            authors.Should().ContainEquivalentOf(author, opt => opt.Excluding(a => a.IdentityUser));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAuthor_WhenAuthorExists()
        {
            // Arrange
            var birthDate = DateTime.MinValue;
            var author = RepositoriesFakeData.GenerateAuthors(1, 1).First();
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();

            author.BirthDate = birthDate;

            // Act
            await _sut.UpdateAsync(author);

            // Assert
            var updatedAuthor = await _sut.GetByIdAsync(author.Id);
            updatedAuthor.Should().BeEquivalentTo(author, opt => opt.Excluding(a => a.IdentityUser));
            updatedAuthor!.BirthDate.Should().Be(birthDate);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAuthor_WhenAuthorExists()
        {
            // Arrange
            var author = RepositoriesFakeData.GenerateAuthors(1, 1).First();
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();

            // Act
            await _sut.DeleteAsync(author.Id);

            // Assert
            _dbContext.Authors.AsNoTracking().Should().NotContainEquivalentOf(author);
        }

        [Fact]
        public async Task GetAuthorByEmailAsync_ShouldReturnNull_WhenAuthorDoesNotExist()
        {
            // Arrange
            var randomEmail = "email";

            // Act
            var result = await _sut.GetAuthorByEmailAsync(randomEmail);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [ClassData(typeof(AuthorsTestData))]
        public async Task GetAuthorByEmailAsync_ShouldReturnAuthor_WhenAuthorExists(Author author)
        {
            // Arrange
            var email = author.Email;

            // Act
            var result = await _sut.GetAuthorByEmailAsync(email);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(author,
                opt => opt
                    .Excluding(a => a.IdentityUser)
                    .Excluding(a => a.News));
        }

        [Fact]
        public void IsEmailUnique_ShouldReturnTrue_WhenNoEmailExists()
        {
            // Arrange
            var email = string.Empty;

            // Act
            var result = _sut.IsEmailUnique(email);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(AuthorsTestData))]
        public void IsEmailUnique_ShouldReturnFalse_WhenEmailExists(Author author)
        {
            // Arrange
            var email = author.Email;

            // Act
            var result = _sut.IsEmailUnique(email);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsFullNameUnique_ShouldReturnTrue_WhenNoFullNameExists()
        {
            // Arrange
            var email = string.Empty;

            // Act
            var result = _sut.IsFullNameUnique(email);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(AuthorsTestData))]
        public void IsFullNameUnique_ShouldReturnFalse_WhenFullNameExists(Author author)
        {
            // Arrange
            var email = author.FullName;

            // Act
            var result = _sut.IsFullNameUnique(email);

            // Assert
            result.Should().BeFalse();
        }
    }
}
*/

