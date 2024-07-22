using NewsSite.DAL.DTO.Request.Auth;

namespace NewsSite.UnitTests.TestData.Validators.Auth.UserRegisterRequestData;

public class UserRegisterRequestValidatorWrongData : TheoryData<UserRegisterRequest>
{
    public UserRegisterRequestValidatorWrongData()
    {
        Add(new UserRegisterRequest
        {
            Email = "WrongEmail",
            BirthDate = DateTime.MaxValue,
            FullName = "   wrong full name",
            Password = "wrong password"
        });
        Add(new UserRegisterRequest
        {
            Email = "wrong@email.",
            BirthDate = DateTime.UtcNow,
            FullName = string.Empty,
            Password = string.Empty
        });
    }
}