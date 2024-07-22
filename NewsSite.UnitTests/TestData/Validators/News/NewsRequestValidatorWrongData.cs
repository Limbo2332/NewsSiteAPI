namespace NewsSite.UnitTests.TestData.Validators.News;

public class NewsRequestValidatorWrongData : TheoryData<string, string>
{
    public NewsRequestValidatorWrongData()
    {
        Add(string.Empty, string.Empty);
        Add("sub", "con");
    }
}