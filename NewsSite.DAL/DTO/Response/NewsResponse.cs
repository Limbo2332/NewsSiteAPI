using NewsSite.DAL.DTO.Response.Abstract;

namespace NewsSite.DAL.DTO.Response;

public class NewsResponse : BaseResponse
{
    public Guid AuthorId { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}