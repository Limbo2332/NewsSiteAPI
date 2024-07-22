namespace NewsSite.UnitTests.TestData.PageSettings.Rubrics;

public class RubricsTestData : TheoryData<Rubric>
{
    public RubricsTestData()
    {
        foreach (var rubric in RepositoriesFakeData.Rubrics) Add(rubric);
    }
}