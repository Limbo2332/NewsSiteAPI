/*
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;
using NewsSite.DAL.Repositories;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.UnitTests.TestData;

namespace NewsSite.IntegrationTests.Systems.Repositories
{
    [Collection(nameof(WebFactoryFixture))]
    public class TagsRepositoryTests
    {
        private readonly OnlineNewsContext _dbContext;
        private readonly INewsRepository _newsRepository;
        private readonly ITagsRepository _sut;

        public TagsRepositoryTests(WebFactoryFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _newsRepository = new NewsRepository(_dbContext);

            _sut = new TagsRepository(_dbContext, _newsRepository);
        }

        [Fact]
        public async Task AddTagForNewsIdAsync_ShouldReturnNull_WhenNoTagAndNews()
        {
            // Arrange
            var tagId = Guid.Empty;
            var newsId = Guid.Empty;

            // Act
            var result = await _sut.AddTagForNewsIdAsync(tagId, newsId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddTagForNewsIdAsync_ShouldAddTag()
        {
            // Arrange
            var tagId = _dbContext.Tags.AsNoTracking().Last().Id;
            var newsId = _newsRepository.GetAll().First().Id;

            var expectedNewsTag = new NewsTags
            {
                NewsId = newsId,
                TagId = tagId
            };

            // Act
            var result = await _sut.AddTagForNewsIdAsync(tagId, newsId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedNewsTag, opt => opt
                .Excluding(a => a.News)
                .Excluding(a => a.Tag));

            var newsTags = _dbContext.NewsTags.AsNoTracking();

            newsTags.Should().ContainEquivalentOf(expectedNewsTag);
        }

        [Fact]
        public async Task DeleteTagForNewsIdAsync_ShouldDelete()
        {
            // Arrange
            var newsTags = await _dbContext.NewsTags.AsNoTracking().FirstAsync();

            // Act
            await _sut.DeleteTagForNewsIdAsync(newsTags.TagId, newsTags.NewsId);

            // Assert
            _dbContext.NewsTags
                .AsNoTracking()
                .Should()
                .NotContainEquivalentOf(newsTags);
        }
    }
}
*/

