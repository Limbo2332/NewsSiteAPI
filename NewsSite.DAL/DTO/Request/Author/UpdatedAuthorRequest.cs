namespace NewsSite.DAL.DTO.Request.Author;

public class UpdatedAuthorRequest
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public bool? Sex { get; set; }

    public string? PublicInformation { get; set; }

    public DateTime BirthDate { get; set; }
}