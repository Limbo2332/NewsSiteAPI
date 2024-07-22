using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.BLL.Interfaces;

public interface ITagsService
{
    Task<PageList<TagResponse>> GetTagsAsync(PageSettings? pageSettings);

    Task<TagResponse> GetTagByIdAsync(Guid id);

    Task<TagResponse> AddTagForNewsIdAsync(NewsTagRequest newsTagRequest);

    Task<TagResponse> CreateNewTagAsync(NewTagRequest newTag);

    Task<TagResponse> UpdateTagAsync(UpdateTagRequest newTag);

    Task DeleteTagAsync(Guid id);

    Task DeleteTagForNewsIdAsync(Guid tagId, Guid newsId);
}