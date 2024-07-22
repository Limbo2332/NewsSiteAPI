namespace NewsSite.DAL.DTO.Request.News;

public class UpdateNewsRequest
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}