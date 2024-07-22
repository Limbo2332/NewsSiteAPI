namespace NewsSite.DAL.DTO.Response;

public class LoginUserResponse
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}