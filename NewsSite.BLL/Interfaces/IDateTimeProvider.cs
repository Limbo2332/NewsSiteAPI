namespace NewsSite.BLL.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}