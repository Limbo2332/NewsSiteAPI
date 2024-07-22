using NewsSite.DAL.Constants;

namespace NewsSite.BLL.Exceptions;

public class InvalidEmailOrPasswordException() : Exception(ValidationMessages.INVALID_EMAIL_OR_PASSWORD_MESSAGE);