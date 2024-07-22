namespace NewsSite.UnitTests.TestData.PageSettings.Authors;

public class AuthorsTestData : TheoryData<Author>
{
    public AuthorsTestData()
    {
        foreach (var author in RepositoriesFakeData.Authors) Add(author);
    }
}