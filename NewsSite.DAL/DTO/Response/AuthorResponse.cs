using NewsSite.DAL.DTO.Response.Abstract;

namespace NewsSite.DAL.DTO.Response;

public class AuthorResponse : BaseResponse
{
    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public bool? Sex { get; set; }

    public string? PublicInformation { get; set; }

    public DateTime BirthDate { get; set; }
}