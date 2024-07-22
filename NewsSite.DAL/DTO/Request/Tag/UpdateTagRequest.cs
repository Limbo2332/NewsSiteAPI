namespace NewsSite.DAL.DTO.Request.Tag;

public class UpdateTagRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}