namespace NewsSite.DAL.DTO.Request.Auth;

public class UserRegisterRequest
{
    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public bool? Sex { get; set; }

    public string Password { get; set; } = string.Empty;

    public string? PublicInformation { get; set; }

    public DateTime BirthDate { get; set; }
}