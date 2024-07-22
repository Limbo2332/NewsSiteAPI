namespace NewsSite.UnitTests.TestData.PageSettings.Tags;

public class TagsIdsTestData : TheoryData<List<Guid>>
{
    public TagsIdsTestData()
    {
        foreach (var tag in RepositoriesFakeData.Tags) Add(new List<Guid> { tag.Id });
    }
}