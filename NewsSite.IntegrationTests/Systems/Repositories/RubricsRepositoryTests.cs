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
    public class RubricsRepositoryTests
    {
        private readonly OnlineNewsContext _dbContext;
        private readonly INewsRepository _newsRepository;
        private readonly IRubricsRepository _sut;

        public RubricsRepositoryTests(WebFactoryFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _newsRepository = new NewsRepository(_dbContext);

            _sut = new RubricsRepository(_dbContext, _newsRepository);
        }

        [Fact]
        public async Task AddRubricForNewsIdAsync_ShouldReturnNull_WhenNoRubricAndNews()
        {
            // Arrange
            var rubricId = Guid.Empty;
            var newsId = Guid.Empty;

            // Act
            var result = await _sut.AddRubricForNewsIdAsync(rubricId, newsId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddRubricForNewsIdAsync_ShouldAddRubric()
        {
            // Arrange
            var rubricId = _dbContext.Rubrics.First().Id;
            var newsId = _newsRepository.GetAll().First().Id;

            var expectedNewsRubric = new NewsRubrics
            {
                NewsId = newsId,
                RubricId = rubricId
            };

            // Act
            var result = await _sut.AddRubricForNewsIdAsync(rubricId, newsId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedNewsRubric, opt => opt
                .Excluding(a => a.News)
                .Excluding(a => a.Rubric));

            var newsRubrics = _dbContext.NewsRubrics.AsNoTracking();

            newsRubrics.Should().ContainEquivalentOf(expectedNewsRubric);
        }

        [Fact]
        public async Task DeleteRubricForNewsIdAsync_ShouldDelete()
        {
            // Arrange
            var newsRubrics = await _dbContext.NewsRubrics.FirstAsync();

            // Act
            await _sut.DeleteRubricForNewsIdAsync(newsRubrics.RubricId, newsRubrics.NewsId);

            // Assert
            _dbContext.NewsRubrics
                .AsNoTracking()
                .Should()
                .NotContainEquivalentOf(newsRubrics);
        }
    }
}
*/

