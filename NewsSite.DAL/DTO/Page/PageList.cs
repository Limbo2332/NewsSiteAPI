namespace NewsSite.DAL.DTO.Page;

public class PageList<T> where T : class
{
    private const int MAX_PAGE_SIZE = 100;
    public const int DefaultPageSize = 30;
    private int _pageSize = DefaultPageSize;

    public List<T> Items { get; set; } = null!;

    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
    }

    public bool HasNextPage => PageSize * PageNumber < TotalCount;

    public bool HasPreviousPage => PageNumber > 1;
}