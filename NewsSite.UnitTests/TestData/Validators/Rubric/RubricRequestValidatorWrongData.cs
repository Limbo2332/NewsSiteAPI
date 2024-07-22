namespace NewsSite.UnitTests.TestData.Validators.Rubric;

public class RubricRequestValidatorWrongData : TheoryData<string>
{
    public RubricRequestValidatorWrongData()
    {
        Add(string.Empty);
        Add("ne");
    }
}