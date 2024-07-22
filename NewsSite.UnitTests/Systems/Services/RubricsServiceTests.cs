using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Services;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Repositories.Base;
using NewsSite.UnitTests.Systems.Services.Abstract;
using NewsSite.UnitTests.TestData;
using NewsSite.UnitTests.TestData.PageSettings.Rubrics;

namespace NewsSite.UnitTests.Systems.Services;

public class RubricsServiceTests : BaseEntityServiceTests<Rubric, RubricResponse>
{
    private readonly Mock<IRubricsRepository> _rubricsRepositoryMock;

    public RubricsServiceTests()
    {
        _rubricsRepositoryMock = new Mock<IRubricsRepository>();
        QueryableMock = RepositoriesFakeData.Rubrics.BuildMock();

        Entities = RepositoriesFakeData.Rubrics;

        _rubricsRepositoryMock
            .Setup(ar => ar.GetAll())
            .Returns(QueryableMock);

        Sut = new RubricsService(
            _userManagerMock.Object,
            _mapper,
            _rubricsRepositoryMock.Object);
    }

    protected override IQueryable<Rubric> QueryableMock { get; }

    protected override RubricsService Sut { get; }

    protected override List<Rubric> Entities { get; }

    [Fact]
    public async Task GetRubricsAsync_ShouldReturnPagedList_WhenNoPageSettings()
    {
        // Arrange
        PageSettings? pageSettings = null;
        var response = _mapper.Map<List<RubricResponse>>(RepositoriesFakeData.Rubrics);

        // Act
        var result = await Sut.GetRubricsAsync(pageSettings);

        // Assert
        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(response);
            result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
            result.TotalCount.Should().Be(RepositoriesFakeData.ItemsCount);
            result.PageSize.Should().Be(PageList<RubricResponse>.DefaultPageSize);
            result.PageNumber.Should().Be(1);
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
        }
    }

    [Theory]
    [ClassData(typeof(RubricsFilteringData))]
    public override Task GetAllAsync_ShouldReturnPagedList_WhenPageFiltering(string propertyName, string propertyValue)
    {
        return base.GetAllAsync_ShouldReturnPagedList_WhenPageFiltering(propertyName, propertyValue);
    }

    [Theory]
    [ClassData(typeof(RubricsSortingData))]
    public override Task GetAllAsync_ShouldReturnPagedList_WhenPageSortingProperty(SortingOrder order,
        string sortingProperty)
    {
        return base.GetAllAsync_ShouldReturnPagedList_WhenPageSortingProperty(order, sortingProperty);
    }

    [Fact]
    public async Task GetRubricByIdAsync_ShouldThrowException_WhenNoRubric()
    {
        // Arrange
        var rubricId = Guid.Empty;
        var exceptionMessage = new NotFoundException(nameof(Rubric), rubricId).Message;

        // Act
        var action = async () => await Sut.GetRubricByIdAsync(rubricId);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(exceptionMessage);
    }

    [Theory]
    [ClassData(typeof(RubricsTestData))]
    public async Task GetRubricByIdAsync_ShouldReturnRubric_WhenIdExists(Rubric rubric)
    {
        // Arrange
        _rubricsRepositoryMock
            .Setup(nr => nr.GetByIdAsync(rubric.Id))
            .ReturnsAsync(rubric);

        var rubricResponse = _mapper.Map<RubricResponse>(rubric);

        // Act
        var result = await Sut.GetRubricByIdAsync(rubric.Id);

        // Assert
        result.Should().BeEquivalentTo(rubricResponse);
    }

    [Fact]
    public async Task AddRubricForNewsIdAsync_ShouldThrowException_WhenNoRubric()
    {
        // Arrange
        var newsRubricRequest = new NewsRubricRequest
        {
            RubricId = Guid.Empty,
            NewsId = Guid.Empty
        };
        var exceptionMessage = new NotFoundException(
            nameof(Rubric),
            newsRubricRequest.RubricId,
            nameof(News),
            newsRubricRequest.NewsId).Message;

        // Act
        var action = async () => await Sut.AddRubricForNewsIdAsync(newsRubricRequest);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(exceptionMessage);
    }

    [Theory]
    [ClassData(typeof(RubricsTestData))]
    public async Task AddRubricForNewsIdAsync_ShouldReturnRubric_WhenIdExists(Rubric rubric)
    {
        // Arrange
        var newsId = Guid.Empty;
        var newsRubricsList = RepositoriesFakeData.NewsRubrics.ToList();
        var newNewsRubrics = new NewsRubrics
        {
            RubricId = rubric.Id,
            NewsId = newsId
        };
        var newsRubricRequest = new NewsRubricRequest
        {
            RubricId = rubric.Id,
            NewsId = newsId
        };

        _rubricsRepositoryMock
            .Setup(r => r.GetByIdAsync(rubric.Id))
            .ReturnsAsync(rubric);

        _rubricsRepositoryMock
            .Setup(r => r.AddRubricForNewsIdAsync(rubric.Id, newsId))
            .Callback(() => { newsRubricsList.Add(newNewsRubrics); })
            .ReturnsAsync(newNewsRubrics);

        var rubricResponse = _mapper.Map<RubricResponse>(rubric);

        // Act
        var result = await Sut.AddRubricForNewsIdAsync(newsRubricRequest);

        // Assert
        result.Should().BeEquivalentTo(rubricResponse);
        newsRubricsList.Should().Contain(newNewsRubrics);
    }

    [Fact]
    public async Task CreateNewRubricAsync_ShouldReturnNewRubric_WhenAddingIsSuccessful()
    {
        // Arrange
        var rubricList = RepositoriesFakeData.Rubrics.ToList();

        var newRubric = new NewRubricRequest
        {
            Name = "newRubric"
        };

        var rubric = _mapper.Map<Rubric>(newRubric);

        _rubricsRepositoryMock
            .Setup(nr => nr.AddAsync(It.IsAny<Rubric>()))
            .Callback(() => rubricList.Add(rubric));

        var rubricResponse = _mapper.Map<RubricResponse>(rubric);

        // Act
        var result = await Sut.CreateNewRubricAsync(newRubric);

        // Assert
        result.Should().BeEquivalentTo(rubricResponse);
        rubricList.Should().Contain(rubric);
    }

    [Theory]
    [ClassData(typeof(RubricsTestData))]
    public async Task UpdateRubricAsync_ShouldUpdateRubric_WhenRubricFound(Rubric rubric)
    {
        // Arrange
        var updateRubricRequest = new UpdateRubricRequest
        {
            Id = rubric.Id,
            Name = "name"
        };

        RubricResponse? rubricResponse = null;

        _rubricsRepositoryMock
            .Setup(nr => nr.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(rubric);

        _rubricsRepositoryMock
            .Setup(nr => nr.UpdateAsync(It.IsAny<Rubric>()))
            .Callback(() => rubricResponse = _mapper.Map<RubricResponse>(rubric));

        // Act
        var result = await Sut.UpdateRubricAsync(updateRubricRequest);

        // Assert
        result.Should().Be(rubricResponse);
    }

    [Theory]
    [ClassData(typeof(RubricsTestData))]
    public async Task DeleteRubricAsync_ShouldDeleteRubric_WhenRubricExists(Rubric rubric)
    {
        // Arrange
        var rubricList = RepositoriesFakeData.Rubrics.ToList();

        _rubricsRepositoryMock
            .Setup(ar => ar.DeleteAsync(rubric.Id))
            .Callback(() => rubricList.Remove(rubric));

        // Act
        await Sut.DeleteRubricAsync(rubric.Id);

        // Assert
        rubricList.Should().NotContain(rubric);
    }

    [Fact]
    public async Task DeleteRubricForNewsIdAsync_WhenRubricAndNewsExists()
    {
        // Arrange
        var newsRubricList = RepositoriesFakeData.NewsRubrics.ToList();
        var newsRubric = newsRubricList.First();

        _rubricsRepositoryMock
            .Setup(r => r.DeleteRubricForNewsIdAsync(newsRubric.RubricId, newsRubric.NewsId))
            .Callback(() => newsRubricList.Remove(newsRubric));

        // Act
        await Sut.DeleteRubricForNewsIdAsync(newsRubric.RubricId, newsRubric.NewsId);

        // Assert
        newsRubricList.Should().NotContain(newsRubric);
    }
}