namespace NewsSite.UnitTests.TestData.PageSettings.News;

public class NewsTestData : TheoryData<DAL.Entities.News>
{
    public NewsTestData()
    {
        foreach (var news in RepositoriesFakeData.News) Add(news);
    }
}