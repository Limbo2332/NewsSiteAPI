using NewsSite.DAL.DTO.Page;

namespace NewsSite.UnitTests.TestData.PageSettings.News;

public class NewsSortingData : TheoryData<SortingOrder, string>
{
    public NewsSortingData()
    {
        Add(SortingOrder.Ascending, nameof(DAL.Entities.News.Subject));
        Add(SortingOrder.Ascending, nameof(DAL.Entities.News.Content));
        Add(SortingOrder.Descending, nameof(DAL.Entities.News.Subject));
        Add(SortingOrder.Descending, nameof(DAL.Entities.News.Content));
    }
}