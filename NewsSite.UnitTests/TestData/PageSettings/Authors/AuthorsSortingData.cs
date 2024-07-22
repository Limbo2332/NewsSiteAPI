using NewsSite.DAL.DTO.Page;

namespace NewsSite.UnitTests.TestData.PageSettings.Authors;

public class AuthorsSortingData : TheoryData<SortingOrder, string>
{
    public AuthorsSortingData()
    {
        Add(SortingOrder.Ascending, nameof(Author.Email));
        Add(SortingOrder.Ascending, nameof(Author.FullName));
        Add(SortingOrder.Ascending, nameof(Author.BirthDate));
        Add(SortingOrder.Ascending, nameof(Author.PublicInformation));
        Add(SortingOrder.Descending, nameof(Author.Email));
        Add(SortingOrder.Descending, nameof(Author.FullName));
        Add(SortingOrder.Descending, nameof(Author.BirthDate));
        Add(SortingOrder.Descending, nameof(Author.PublicInformation));
    }
}