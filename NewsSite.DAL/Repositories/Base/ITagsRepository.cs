using NewsSite.DAL.Entities;

namespace NewsSite.DAL.Repositories.Base;

public interface ITagsRepository : IGenericRepository<Tag>
{
    Task<NewsTags?> AddTagForNewsIdAsync(Guid tagId, Guid newsId);

    Task DeleteTagForNewsIdAsync(Guid tagId, Guid newsId);
}