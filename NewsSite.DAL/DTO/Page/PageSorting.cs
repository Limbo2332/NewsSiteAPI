namespace NewsSite.DAL.DTO.Page;

public class PageSorting
{
    public string SortingProperty { get; set; } = string.Empty;

    public SortingOrder SortingOrder { get; set; }
}