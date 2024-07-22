using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.BLL.Interfaces;

public interface INewsService
{
    Task<PageList<NewsResponse>> GetNewsAsync(PageSettings? pageSettings);

    Task<PageList<NewsResponse>> GetNewsByRubricAsync(Guid rubricId, PageSettings? pageSettings);

    Task<PageList<NewsResponse>> GetNewsByTagsAsync(List<Guid> tagsIds, PageSettings? pageSettings);

    Task<PageList<NewsResponse>> GetNewsByAuthorAsync(Guid authorId, PageSettings? pageSettings);

    Task<PageList<NewsResponse>> GetNewsByPeriodOfTimeAsync(DateTime startDate, DateTime endDate,
        PageSettings? pageSettings);

    Task<NewsResponse> GetNewsByIdAsync(Guid id);

    Task<NewsResponse> CreateNewNewsAsync(NewNewsRequest newNewsRequest);

    Task<NewsResponse> UpdateNewsAsync(UpdateNewsRequest updateNewsRequest);

    Task DeleteNewsAsync(Guid newsId);
}