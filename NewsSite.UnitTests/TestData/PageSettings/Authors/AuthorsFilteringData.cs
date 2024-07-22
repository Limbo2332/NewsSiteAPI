namespace NewsSite.UnitTests.TestData.PageSettings.Authors;

public class AuthorsFilteringData : TheoryData<string, string>
{
    public AuthorsFilteringData()
    {
        Add(nameof(Author.Email), RepositoriesFakeData.Authors.First().Email);
        Add(nameof(Author.FullName), RepositoriesFakeData.Authors.First().FullName);
        Add(nameof(Author.PublicInformation), RepositoriesFakeData.Authors.First().PublicInformation!);
    }
}