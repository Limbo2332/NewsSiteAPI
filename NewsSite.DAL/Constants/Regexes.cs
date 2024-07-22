namespace NewsSite.DAL.Constants;

public static class Regexes
{
    public const string EMAIL_REGEX =
        @"^(([^<>()[\]\\.,;:\s@']+(\.[^<>()[\]\\.,;:\s@']+)*)|('.+'))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";

    public const string NO_SPACES_REGEX = @"^(?!\s)(.*\S)?(?<!\s)$";
    public const string FULL_NAME_REGEX = @"^[a-zA-Z\s]+$";
    public const string PASSWORD_REGEX = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{1,}$";
}