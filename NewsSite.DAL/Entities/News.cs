using NewsSite.DAL.Entities.Abstract;

namespace NewsSite.DAL.Entities;

public class News : BaseEntity
{
    public Guid CreatedBy { get; set; }
    public Author? Author { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public IEnumerable<NewsTags>? NewsTags { get; set; }
    public IEnumerable<NewsRubrics>? NewsRubrics { get; set; }
}