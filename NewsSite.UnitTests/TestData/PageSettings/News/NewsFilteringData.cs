namespace NewsSite.UnitTests.TestData.PageSettings.News;

public class NewsFilteringData : TheoryData<string, string>
{
    public NewsFilteringData()
    {
        Add(nameof(DAL.Entities.News.Content), RepositoriesFakeData.News.First().Content);
        Add(nameof(DAL.Entities.News.Subject), RepositoriesFakeData.News.First().Subject);
    }
}