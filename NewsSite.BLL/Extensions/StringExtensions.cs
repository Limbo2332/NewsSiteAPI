using System.Globalization;

namespace NewsSite.BLL.Extensions;

public static class StringExtensions
{
    public static bool IsDateTime(this string value)
    {
        return DateTime.TryParse(value, CultureInfo.InvariantCulture, out _);
    }
}