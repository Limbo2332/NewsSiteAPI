using NewsSite.DAL.DTO.Page;

namespace NewsSite.UnitTests.TestData.PageSettings.Rubrics;

public class RubricsSortingData : TheoryData<SortingOrder, string>
{
    public RubricsSortingData()
    {
        Add(SortingOrder.Ascending, nameof(Rubric.Name));
        Add(SortingOrder.Descending, nameof(Rubric.Name));
    }
}