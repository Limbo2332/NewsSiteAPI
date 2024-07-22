using NewsSite.DAL.DTO.Page;

namespace NewsSite.DAL.DTO.Request.News;

public class NewsByPeriodOfDateRequest
{
    public PageSettings? PageSettings { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}