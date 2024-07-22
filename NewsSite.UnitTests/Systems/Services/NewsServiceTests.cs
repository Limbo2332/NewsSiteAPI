using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Services;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Repositories.Base;
using NewsSite.UnitTests.Systems.Services.Abstract;
using NewsSite.UnitTests.TestData;
using NewsSite.UnitTests.TestData.PageSettings.Authors;
using NewsSite.UnitTests.TestData.PageSettings.News;
using NewsSite.UnitTests.TestData.PageSettings.Rubrics;
using NewsSite.UnitTests.TestData.PageSettings.Tags;

namespace NewsSite.UnitTests.Systems.Services;

public class NewsServiceTests : BaseEntityServiceTests<News, NewsResponse>
{
    private readonly Mock<IAuthorsRepository> _authorsRepositoryMock;
    private readonly Mock<INewsRepository> _newsRepositoryMock;

    public NewsServiceTests()
    {
        _newsRepositoryMock = new Mock<INewsRepository>();
        _authorsRepositoryMock = new Mock<IAuthorsRepository>();
        QueryableMock = RepositoriesFakeData.News.BuildMock();

        Entities = RepositoriesFakeData.News;

        _newsRepositoryMock
            .Setup(ar => ar.GetAll())
            .Returns(QueryableMock);

        Sut = new NewsService(
            _userManagerMock.Object,
            _mapper,
            _newsRepositoryMock.Object,
            _authorsRepositoryMock.Object);
    }

    protected override IQueryable<News> QueryableMock { get; }

    protected override NewsService Sut { get; }

    protected override List<News> Entities { get; }

    [Fact]
    public async Task GetNewsAsync_ShouldReturnPagedList_WhenNoPageSettings()
    {
        // Arrange
        PageSettings? pageSettings = null;
        var response = _mapper.Map<List<NewsResponse>>(RepositoriesFakeData.News);

        // Act
        var result = await Sut.GetNewsAsync(pageSettings);

        // Assert
        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(response);
            result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
            result.TotalCount.Should().Be(RepositoriesFakeData.ItemsCount);
            result.PageSize.Should().Be(PageList<NewsResponse>.DefaultPageSize);
            result.PageNumber.Should().Be(1);
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
        }
    }

    [Theory]
    [ClassData(typeof(NewsFilteringData))]
    public override Task GetAllAsync_ShouldReturnPagedList_WhenPageFiltering(string propertyName, string propertyValue)
    {
        return base.GetAllAsync_ShouldReturnPagedList_WhenPageFiltering(propertyName, propertyValue);
    }

    [Theory]
    [ClassData(typeof(NewsSortingData))]
    public override async Task GetAllAsync_ShouldReturnPagedList_WhenPageSortingProperty(SortingOrder order,
        string sortingProperty)
    {
        await base.GetAllAsync_ShouldReturnPagedList_WhenPageSortingProperty(order, sortingProperty);
    }

    [Theory]
    [ClassData(typeof(RubricsTestData))]
    public async Task GetNewsByRubricAsync_ShouldReturnPagedListByRubric_WhenNoPageSettings(Rubric rubric)
    {
        // Arrange
        PageSettings? pageSettings = null;

        var newsWithRubrics = RepositoriesFakeData.News.GroupJoin(
                RepositoriesFakeData.NewsRubrics,
                n => n.Id,
                nr => nr.NewsId,
                (news, newsRubrics) =>
                {
                    news.NewsRubrics = newsRubrics.ToList();
                    return news;
                })
            .BuildMock();

        _newsRepositoryMock
            .Setup(nr => nr.GetAll())
            .Returns(newsWithRubrics);

        var response = _mapper.Map<List<NewsResponse>>(
            newsWithRubrics
                .Where(n => n.NewsRubrics!.Any(nr => nr.RubricId == rubric.Id)));

        // Act
        var result = await Sut.GetNewsByRubricAsync(rubric.Id, pageSettings);

        // Assert
        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(response);
            result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
            result.PageSize.Should().Be(PageList<NewsResponse>.DefaultPageSize);
            result.PageNumber.Should().Be(1);
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
        }
    }

    [Theory]
    [ClassData(typeof(TagsIdsTestData))]
    public async Task GetNewsByTagsIdsAsync_ShouldReturnPagedListByTagsIds_WhenNoPageSettings(List<Guid> tagsIds)
    {
        // Arrange
        PageSettings? pageSettings = null;

        var newsWithTags = RepositoriesFakeData.News.GroupJoin(
                RepositoriesFakeData.NewsTags,
                n => n.Id,
                nt => nt.NewsId,
                (news, newsTags) =>
                {
                    news.NewsTags = newsTags.ToList();
                    return news;
                })
            .BuildMock();

        _newsRepositoryMock
            .Setup(nr => nr.GetAll())
            .Returns(newsWithTags);

        var response = _mapper.Map<List<NewsResponse>>(
            newsWithTags
                .Where(n => n.NewsTags!.Any(nt => tagsIds.Contains(nt.TagId))));

        // Act
        var result = await Sut.GetNewsByTagsAsync(tagsIds, pageSettings);

        // Assert
        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(response);
            result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
            result.PageSize.Should().Be(PageList<NewsResponse>.DefaultPageSize);
            result.PageNumber.Should().Be(1);
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
        }
    }

    [Theory]
    [ClassData(typeof(AuthorsTestData))]
    public async Task GetNewsByAuthorAsync_ShouldReturnPagedListByTagsIds_WhenNoPageSettings(Author author)
    {
        // Arrange
        PageSettings? pageSettings = null;

        var response = _mapper.Map<List<NewsResponse>>(
            RepositoriesFakeData.News
                .Where(n => n.CreatedBy == author.Id));

        // Act
        var result = await Sut.GetNewsByAuthorAsync(author.Id, pageSettings);

        // Assert
        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(response);
            result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
            result.PageSize.Should().Be(PageList<NewsResponse>.DefaultPageSize);
            result.PageNumber.Should().Be(1);
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
        }
    }

    [Fact]
    public async Task GetNewsByPeriodOfTimeAsync_ShouldReturnPagedListByTagsIds_WhenNoPageSettings()
    {
        // Arrange
        PageSettings? pageSettings = null;

        var startDate = DateTime.UtcNow.AddYears(-1);
        var endDate = DateTime.UtcNow.AddYears(1);

        var response = _mapper.Map<List<NewsResponse>>(
            RepositoriesFakeData.News
                .Where(n => n.UpdatedAt >= startDate && n.UpdatedAt <= endDate)
                .ToList());

        // Act
        var result = await Sut.GetNewsByPeriodOfTimeAsync(startDate, endDate, pageSettings);

        // Assert
        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(response);
            result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
            result.PageSize.Should().Be(PageList<NewsResponse>.DefaultPageSize);
            result.PageNumber.Should().Be(1);
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
        }
    }

    [Fact]
    public async Task GetNewsByIdAsync_ShouldThrowException_WhenNoNews()
    {
        // Arrange
        var newsId = Guid.Empty;
        var exceptionMessage = new NotFoundException(nameof(News), newsId).Message;

        // Act
        var action = async () => await Sut.GetNewsByIdAsync(newsId);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(exceptionMessage);
    }

    [Theory]
    [ClassData(typeof(NewsTestData))]
    public async Task GetNewsByIdAsync_ShouldReturnNews_WhenIdExists(News news)
    {
        // Arrange
        _newsRepositoryMock
            .Setup(nr => nr.GetByIdAsync(news.Id))
            .ReturnsAsync(news);

        var newsResponse = _mapper.Map<NewsResponse>(news);

        // Act
        var result = await Sut.GetNewsByIdAsync(news.Id);

        // Assert
        result.Should().BeEquivalentTo(newsResponse);
    }

    [Fact]
    public async Task CreateNewNewsAsync_ShouldThrowException_WhenNoAuthor()
    {
        // Arrange
        var authorId = Guid.Empty;
        var exceptionMessage = new NotFoundException(nameof(Author), authorId).Message;

        var newNews = new NewNewsRequest
        {
            AuthorId = authorId
        };

        // Act
        var action = async () => await Sut.CreateNewNewsAsync(newNews);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(exceptionMessage);
    }

    [Fact]
    public async Task CreateNewNewsAsync_ShouldReturnNewNews_WhenNoRubricAndTags()
    {
        // Arrange
        var newsList = RepositoriesFakeData.News.ToList();
        var author = RepositoriesFakeData.Authors.First();

        var newNews = new NewNewsRequest
        {
            AuthorId = author.Id,
            Subject = "subject",
            Content = "content"
        };

        var news = _mapper.Map<News>(newNews);

        _authorsRepositoryMock
            .Setup(r => r.GetByIdAsync(newNews.AuthorId))
            .ReturnsAsync(author);

        _newsRepositoryMock
            .Setup(nr => nr.AddAsync(It.IsAny<News>()))
            .Callback(() => newsList.Add(news));

        var newsResponse = _mapper.Map<NewsResponse>(news);

        // Act
        var result = await Sut.CreateNewNewsAsync(newNews);

        // Assert
        result.Should().BeEquivalentTo(newsResponse);
        newsList.Should().Contain(news);
    }

    [Fact]
    public async Task CreateNewNewsAsync_ShouldReturnNewNews_WhenTagsAndRubrics()
    {
        // Arrange
        var author = RepositoriesFakeData.Authors.First();

        var tagsIds = RepositoriesFakeData.Tags.Select(t => t.Id).ToList();
        var rubricsIds = RepositoriesFakeData.Rubrics.Select(t => t.Id).ToList();

        var newNews = new NewNewsRequest
        {
            AuthorId = author.Id,
            Subject = "subject",
            Content = "content",
            TagsIds = tagsIds,
            RubricsIds = rubricsIds
        };

        var news = _mapper.Map<News>(newNews);
        news.NewsTags = new List<NewsTags>();
        news.NewsRubrics = new List<NewsRubrics>();

        _authorsRepositoryMock
            .Setup(r => r.GetByIdAsync(newNews.AuthorId))
            .ReturnsAsync(author);

        _newsRepositoryMock
            .Setup(nr => nr.AddNewsTagsAsync(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
            .Callback(() =>
            {
                news.NewsTags =
                    tagsIds.Select(tagId => new NewsTags
                    {
                        TagId = tagId,
                        NewsId = news.Id
                    });
            });

        _newsRepositoryMock
            .Setup(nr => nr.AddNewsRubricsAsync(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
            .Callback(() =>
            {
                news.NewsRubrics =
                    rubricsIds.Select(rubricId => new NewsRubrics
                    {
                        RubricId = rubricId,
                        NewsId = news.Id
                    });
            });

        var newsResponse = _mapper.Map<NewsResponse>(news);

        // Act
        var result = await Sut.CreateNewNewsAsync(newNews);

        // Assert
        result.Should().BeEquivalentTo(newsResponse);
        news.NewsTags.Count().Should().Be(tagsIds.Count);
        news.NewsRubrics.Count().Should().Be(rubricsIds.Count);
    }

    [Theory]
    [ClassData(typeof(NewsTestData))]
    public async Task UpdateNewsAsync_ShouldUpdateNews_WhenNewsFound(News news)
    {
        // Arrange
        var updateNewsRequest = new UpdateNewsRequest
        {
            Id = news.Id,
            Content = "content",
            Subject = "subject"
        };

        NewsResponse? newsResponse = null;

        _newsRepositoryMock
            .Setup(nr => nr.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(news);

        _newsRepositoryMock
            .Setup(nr => nr.UpdateAsync(It.IsAny<News>()))
            .Callback(() => newsResponse = _mapper.Map<NewsResponse>(news));

        // Act
        var result = await Sut.UpdateNewsAsync(updateNewsRequest);

        // Assert
        result.Should().Be(newsResponse);
    }

    [Theory]
    [ClassData(typeof(NewsTestData))]
    public async Task DeleteNewsAsync_ShouldDeleteNews_WhenNewsExists(News news)
    {
        // Arrange
        var newsList = RepositoriesFakeData.News.ToList();

        _newsRepositoryMock
            .Setup(ar => ar.DeleteAsync(news.Id))
            .Callback(() => newsList.Remove(news));

        // Act
        await Sut.DeleteNewsAsync(news.Id);

        // Assert
        newsList.Should().NotContain(news);
    }
}