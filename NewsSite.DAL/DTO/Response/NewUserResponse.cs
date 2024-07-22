namespace NewsSite.DAL.DTO.Response;

public class NewUserResponse
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string? PublicInformation { get; set; }

    public DateTime BirthDate { get; set; }

    public string Token { get; set; } = string.Empty;
}