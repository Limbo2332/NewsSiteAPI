namespace NewsSite.DAL.DTO.Page;

public class PageSettings
{
    public PageSorting? PageSorting { get; set; }

    public PageFiltering? PageFiltering { get; set; }

    public PagePagination? PagePagination { get; set; }
}