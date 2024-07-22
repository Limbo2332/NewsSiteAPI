using NewsSite.DAL.DTO.Response.Abstract;

namespace NewsSite.DAL.DTO.Response;

public class RubricResponse : BaseResponse
{
    public string Name { get; set; } = string.Empty;
}