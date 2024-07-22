namespace NewsSite.UnitTests.TestData.Validators.Tag;

public class TagRequestValidatorWrongData : TheoryData<string>
{
    public TagRequestValidatorWrongData()
    {
        Add(string.Empty);
        Add("ne");
    }
}