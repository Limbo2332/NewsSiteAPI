using NewsSite.DAL.Entities;

namespace NewsSite.DAL.Repositories.Base;

public interface INewsRepository : IGenericRepository<News>
{
    Task AddNewsRubricsAsync(Guid newsId, List<Guid> rubricId);

    Task AddNewsTagsAsync(Guid newsId, List<Guid> tagsIds);
}