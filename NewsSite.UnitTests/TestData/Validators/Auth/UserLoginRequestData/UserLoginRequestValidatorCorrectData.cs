using NewsSite.DAL.DTO.Request.Auth;

namespace NewsSite.UnitTests.TestData.Validators.Auth.UserLoginRequestData;

public class UserLoginRequestValidatorCorrectData : TheoryData<UserLoginRequest>
{
    public UserLoginRequestValidatorCorrectData()
    {
        Add(new UserLoginRequest
        {
            Email = "correctEmail@gmail.com",
            Password = "correctPassword123$"
        });
        Add(new UserLoginRequest
        {
            Email = "correct@ukr.ua",
            Password = "123456654321$aA"
        });
    }
}