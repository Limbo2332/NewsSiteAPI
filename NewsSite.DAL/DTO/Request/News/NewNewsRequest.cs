namespace NewsSite.DAL.DTO.Request.News;

public class NewNewsRequest
{
    public Guid AuthorId { get; set; }

    public List<Guid>? RubricsIds { get; set; }

    public List<Guid>? TagsIds { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}