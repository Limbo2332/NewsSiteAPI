using NewsSite.DAL.Context.Constants;

namespace NewsSite.UnitTests.TestData.Validators.News;

public class NewsRequestValidatorCorrectData : TheoryData<string, string>
{
    public NewsRequestValidatorCorrectData()
    {
        Add("Subject", new Faker().Random.String2(ConfigurationConstants.CONTENT_MAXLENGTH - 1));
        Add("Subject Subject", new Faker().Random.String2(ConfigurationConstants.CONTENT_MAXLENGTH - 1));
    }
}