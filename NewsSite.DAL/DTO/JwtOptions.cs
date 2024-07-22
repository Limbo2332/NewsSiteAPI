namespace NewsSite.DAL.DTO;

public class JwtOptions
{
    public static string SectionName = "JWT";

    public required string SecretKey { get; set; }

    public required string Issuer { get; set; }

    public required string Audience { get; set; }

    public int ExpiresInMinutes { get; set; }
}