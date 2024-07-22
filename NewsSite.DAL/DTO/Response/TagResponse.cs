using NewsSite.DAL.DTO.Response.Abstract;

namespace NewsSite.DAL.DTO.Response;

public class TagResponse : BaseResponse
{
    public string Name { get; set; } = string.Empty;
}