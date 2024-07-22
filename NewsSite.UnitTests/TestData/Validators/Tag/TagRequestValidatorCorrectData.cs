namespace NewsSite.UnitTests.TestData.Validators.Tag;

public class TagRequestValidatorCorrectData : TheoryData<string>
{
    public TagRequestValidatorCorrectData()
    {
        Add("tagTagName");
        Add("tagNameTag");
    }
}