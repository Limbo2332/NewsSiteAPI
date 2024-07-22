using NewsSite.DAL.DTO.Request.Auth;

namespace NewsSite.UnitTests.TestData.Validators.Auth.UserLoginRequestData;

public class UserLoginRequestValidatorWrongData : TheoryData<UserLoginRequest>
{
    public UserLoginRequestValidatorWrongData()
    {
        Add(new UserLoginRequest
        {
            Email = string.Empty,
            Password = string.Empty
        });
        Add(new UserLoginRequest
        {
            Email = "string.Empty",
            Password = "string.Empty"
        });
        Add(new UserLoginRequest
        {
            Email = "wrong@email",
            Password = "wrongPassword1"
        });
    }
}