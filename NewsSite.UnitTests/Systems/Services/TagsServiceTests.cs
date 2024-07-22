using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Services;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Repositories.Base;
using NewsSite.UnitTests.Systems.Services.Abstract;
using NewsSite.UnitTests.TestData;
using NewsSite.UnitTests.TestData.PageSettings.Tags;

namespace NewsSite.UnitTests.Systems.Services;

public class TagsServiceTests : BaseEntityServiceTests<Tag, TagResponse>
{
    private readonly Mock<ITagsRepository> _tagsRepositoryMock;

    public TagsServiceTests()
    {
        _tagsRepositoryMock = new Mock<ITagsRepository>();
        QueryableMock = RepositoriesFakeData.Tags.BuildMock();

        Entities = RepositoriesFakeData.Tags;

        _tagsRepositoryMock
            .Setup(ar => ar.GetAll())
            .Returns(QueryableMock);

        Sut = new TagsService(
            _userManagerMock.Object,
            _mapper,
            _tagsRepositoryMock.Object);
    }

    protected sealed override IQueryable<Tag> QueryableMock { get; }

    protected override TagsService Sut { get; }

    protected override List<Tag> Entities { get; }

    [Fact]
    public async Task GetTagsAsync_ShouldReturnPagedList_WhenNoPageSettings()
    {
        // Arrange
        PageSettings? pageSettings = null;
        var response = _mapper.Map<List<TagResponse>>(RepositoriesFakeData.Tags);

        // Act
        var result = await Sut.GetTagsAsync(pageSettings);

        // Assert
        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(response);
            result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
            result.TotalCount.Should().Be(RepositoriesFakeData.ItemsCount);
            result.PageSize.Should().Be(PageList<TagResponse>.DefaultPageSize);
            result.PageNumber.Should().Be(1);
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
        }
    }

    [Theory]
    [ClassData(typeof(TagsFilteringData))]
    public override Task GetAllAsync_ShouldReturnPagedList_WhenPageFiltering(string propertyName, string propertyValue)
    {
        return base.GetAllAsync_ShouldReturnPagedList_WhenPageFiltering(propertyName, propertyValue);
    }

    [Theory]
    [ClassData(typeof(TagsSortingData))]
    public override Task GetAllAsync_ShouldReturnPagedList_WhenPageSortingProperty(SortingOrder order,
        string sortingProperty)
    {
        return base.GetAllAsync_ShouldReturnPagedList_WhenPageSortingProperty(order, sortingProperty);
    }

    [Fact]
    public async Task GetTagByIdAsync_ShouldThrowException_WhenNoTag()
    {
        // Arrange
        var tagId = Guid.Empty;
        var exceptionMessage = new NotFoundException(nameof(Tag), tagId).Message;

        // Act
        var action = async () => await Sut.GetTagByIdAsync(tagId);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(exceptionMessage);
    }

    [Theory]
    [ClassData(typeof(TagsTestData))]
    public async Task GetTagByIdAsync_ShouldReturnTag_WhenIdExists(Tag tag)
    {
        // Arrange
        _tagsRepositoryMock
            .Setup(nr => nr.GetByIdAsync(tag.Id))
            .ReturnsAsync(tag);

        var tagResponse = _mapper.Map<TagResponse>(tag);

        // Act
        var result = await Sut.GetTagByIdAsync(tag.Id);

        // Assert
        result.Should().BeEquivalentTo(tagResponse);
    }

    [Fact]
    public async Task AddTagForNewsIdAsync_ShouldThrowException_WhenNoTag()
    {
        // Arrange
        var newNewsTagRequest = new NewsTagRequest
        {
            TagId = Guid.Empty,
            NewsId = Guid.Empty
        };
        var exceptionMessage = new NotFoundException(
            nameof(Tag),
            newNewsTagRequest.TagId,
            nameof(News),
            newNewsTagRequest.NewsId).Message;

        // Act
        var action = async () => await Sut.AddTagForNewsIdAsync(newNewsTagRequest);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(exceptionMessage);
    }

    [Theory]
    [ClassData(typeof(TagsTestData))]
    public async Task AddTagForNewsIdAsync_ShouldReturnTag_WhenIdExists(Tag tag)
    {
        // Arrange
        var newsId = Guid.Empty;
        var newsTagsList = RepositoriesFakeData.NewsTags.ToList();
        var newNewsTags = new NewsTags
        {
            TagId = tag.Id,
            NewsId = newsId
        };
        var newNewsTagRequest = new NewsTagRequest
        {
            TagId = newNewsTags.TagId,
            NewsId = newsId
        };

        _tagsRepositoryMock
            .Setup(r => r.GetByIdAsync(tag.Id))
            .ReturnsAsync(tag);

        _tagsRepositoryMock
            .Setup(r => r.AddTagForNewsIdAsync(tag.Id, newsId))
            .Callback(() => { newsTagsList.Add(newNewsTags); })
            .ReturnsAsync(newNewsTags);

        var tagResponse = _mapper.Map<TagResponse>(tag);

        // Act
        var result = await Sut.AddTagForNewsIdAsync(newNewsTagRequest);

        // Assert
        result.Should().BeEquivalentTo(tagResponse);
        newsTagsList.Should().Contain(newNewsTags);
    }

    [Fact]
    public async Task CreateNewTagAsync_ShouldReturnNewTag_WhenAddingIsSuccessful()
    {
        // Arrange
        var tagList = RepositoriesFakeData.Tags.ToList();

        var newTag = new NewTagRequest
        {
            Name = "newTag"
        };

        var tag = _mapper.Map<Tag>(newTag);

        _tagsRepositoryMock
            .Setup(nr => nr.AddAsync(It.IsAny<Tag>()))
            .Callback(() => tagList.Add(tag));

        var tagResponse = _mapper.Map<TagResponse>(tag);

        // Act
        var result = await Sut.CreateNewTagAsync(newTag);

        // Assert
        result.Should().BeEquivalentTo(tagResponse);
        tagList.Should().Contain(tag);
    }

    [Theory]
    [ClassData(typeof(TagsTestData))]
    public async Task UpdateTagAsync_ShouldUpdateTag_WhenTagFound(Tag tag)
    {
        // Arrange
        var updateTagRequest = new UpdateTagRequest
        {
            Id = tag.Id,
            Name = "name"
        };

        TagResponse? tagResponse = null;

        _tagsRepositoryMock
            .Setup(nr => nr.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(tag);

        _tagsRepositoryMock
            .Setup(nr => nr.UpdateAsync(It.IsAny<Tag>()))
            .Callback(() => tagResponse = _mapper.Map<TagResponse>(tag));

        // Act
        var result = await Sut.UpdateTagAsync(updateTagRequest);

        // Assert
        result.Should().Be(tagResponse);
    }

    [Theory]
    [ClassData(typeof(TagsTestData))]
    public async Task DeleteTagAsync_ShouldDeleteTag_WhenTagExists(Tag tag)
    {
        // Arrange
        var tagList = RepositoriesFakeData.Tags.ToList();

        _tagsRepositoryMock
            .Setup(ar => ar.DeleteAsync(tag.Id))
            .Callback(() => tagList.Remove(tag));

        // Act
        await Sut.DeleteTagAsync(tag.Id);

        // Assert
        tagList.Should().NotContain(tag);
    }

    [Fact]
    public async Task DeleteTagForNewsIdAsync_WhenTagAndNewsExists()
    {
        // Arrange
        var newsTagList = RepositoriesFakeData.NewsTags.ToList();
        var newsTag = newsTagList.First();

        _tagsRepositoryMock
            .Setup(r => r.DeleteTagForNewsIdAsync(newsTag.TagId, newsTag.NewsId))
            .Callback(() => newsTagList.Remove(newsTag));

        // Act
        await Sut.DeleteTagForNewsIdAsync(newsTag.TagId, newsTag.NewsId);

        // Assert
        newsTagList.Should().NotContain(newsTag);
    }
}