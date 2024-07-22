using NewsSite.DAL.DTO.Page;

namespace NewsSite.UnitTests.TestData.PageSettings.Tags;

public class TagsSortingData : TheoryData<SortingOrder, string>
{
    public TagsSortingData()
    {
        Add(SortingOrder.Ascending, nameof(Tag.Name));
        Add(SortingOrder.Descending, nameof(Tag.Name));
    }
}