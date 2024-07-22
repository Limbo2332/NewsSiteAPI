namespace NewsSite.UnitTests.TestData.Validators.Common;

public class NoSpacesRegexFormatWrongData : TheoryData<string>
{
    public NoSpacesRegexFormatWrongData()
    {
        Add("   fdfsaf");
        Add("fsafsa    ");
    }
}