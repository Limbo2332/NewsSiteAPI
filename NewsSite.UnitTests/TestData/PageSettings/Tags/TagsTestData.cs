namespace NewsSite.UnitTests.TestData.PageSettings.Tags;

public class TagsTestData : TheoryData<Tag>
{
    public TagsTestData()
    {
        foreach (var tag in RepositoriesFakeData.Tags) Add(tag);
    }
}