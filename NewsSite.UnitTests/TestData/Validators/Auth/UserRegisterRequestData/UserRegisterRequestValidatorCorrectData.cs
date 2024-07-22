using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.DTO.Request.Auth;

namespace NewsSite.UnitTests.TestData.Validators.Auth.UserRegisterRequestData;

public class UserRegisterRequestValidatorCorrectData : TheoryData<UserRegisterRequest>
{
    public UserRegisterRequestValidatorCorrectData()
    {
        Add(new UserRegisterRequest
        {
            BirthDate = DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER * 2),
            Email = "correct@email.com",
            FullName = "correctFullName",
            Password = "correctPassword1234$",
            PublicInformation = null
        });
        Add(new UserRegisterRequest
        {
            BirthDate = DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER * 3),
            Email = "correct@ukr.net",
            FullName = "correct FullName",
            Password = "correctPassword1234$",
            PublicInformation = "publicInformation"
        });
    }
}