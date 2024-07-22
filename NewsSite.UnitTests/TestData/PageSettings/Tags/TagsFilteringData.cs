namespace NewsSite.UnitTests.TestData.PageSettings.Tags;

public class TagsFilteringData : TheoryData<string, string>
{
    public TagsFilteringData()
    {
        Add(nameof(Tag.Name), RepositoriesFakeData.Tags.First().Name);
    }
}