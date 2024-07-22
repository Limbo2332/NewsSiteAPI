namespace NewsSite.UnitTests.TestData.PageSettings.Rubrics;

public class RubricsFilteringData : TheoryData<string, string>
{
    public RubricsFilteringData()
    {
        Add(nameof(Rubric.Name), RepositoriesFakeData.Rubrics.First().Name);
    }
}