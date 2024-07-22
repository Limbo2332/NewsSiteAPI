namespace NewsSite.DAL.DTO.Response.Abstract;

public abstract class BaseResponse
{
    public Guid Id { get; set; }

    public DateTime UpdatedAt { get; set; }

    public override bool Equals(object? obj)
    {
        var response = obj as BaseResponse;

        return Id == response?.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}