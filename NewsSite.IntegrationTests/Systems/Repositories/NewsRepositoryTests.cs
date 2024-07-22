/*
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Repositories;
using NewsSite.DAL.Repositories.Base;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.UnitTests.TestData;

namespace NewsSite.IntegrationTests.Systems.Repositories
{
    public class NewsRepositoryTests
    {
        private readonly OnlineNewsContext _dbContext;
        private readonly INewsRepository _sut;

        public NewsRepositoryTests(WebFactoryFixture fixture)
        {
            _dbContext = fixture.DbContext;

            _sut = new NewsRepository(_dbContext);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteNews_WhenNewsExists()
        {
            // Arrange
            var news = RepositoriesFakeData.News.First();

            // Act
            await _sut.DeleteAsync(news.Id);

            // Assert
            _dbContext.News.AsNoTracking().Should().NotContainEquivalentOf(news);
        }

        [Fact]
        public async Task AddNewsRubrics_ShouldCreateNewsRubrics()
        {
            // Arrange
            var newsRubrics = RepositoriesFakeData.GenerateNewsRubrics(10, 999);

            var newsId = newsRubrics.First().NewsId;

            var addedNewsRubrics = newsRubrics
                .Where(r => r.NewsId == newsId)
                .ToList();

            var rubricsIds = addedNewsRubrics
                .Select(r => r.RubricId)
                .ToList();

            // Act
            await _sut.AddNewsRubricsAsync(newsId, rubricsIds);

            // Assert
            _dbContext.NewsRubrics
                .Where(nr => nr.NewsId == newsId)
                .Should()
                .BeEquivalentTo(addedNewsRubrics);
        }

        [Fact]
        public async Task AddNewsTags_ShouldCreateNewsTags()
        {
            // Arrange
            var newsTags = RepositoriesFakeData.GenerateNewsTags(10, 999);

            var newsId = newsTags.First().NewsId;

            var addedNewsTags = newsTags
                .Where(r => r.NewsId == newsId)
                .ToList();

            var rubricsIds = addedNewsTags
                .Select(r => r.TagId)
                .ToList();

            // Act
            await _sut.AddNewsTagsAsync(newsId, rubricsIds);

            // Assert
            _dbContext.NewsTags
                .Where(nr => nr.NewsId == newsId)
                .Should()
                .BeEquivalentTo(addedNewsTags);
        }
    }
}
*/

