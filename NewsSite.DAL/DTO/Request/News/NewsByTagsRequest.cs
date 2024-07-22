using NewsSite.DAL.DTO.Page;

namespace NewsSite.DAL.DTO.Request.News;

public class NewsByTagsRequest
{
    public PageSettings? PageSettings { get; set; }

    public List<Guid> TagsIds { get; set; } = new();
}