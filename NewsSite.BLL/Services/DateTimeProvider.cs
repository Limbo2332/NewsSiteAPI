using NewsSite.BLL.Interfaces;

namespace NewsSite.BLL.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}