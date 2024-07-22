using FluentValidation.TestHelper;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.UI.Validators.Auth;
using NewsSite.UnitTests.TestData.Validators.Auth.UserLoginRequestData;

namespace NewsSite.UnitTests.Systems.Validators.Auth;

public class UserLoginRequestValidatorTests
{
    private readonly UserLoginRequestValidator _sut;

    public UserLoginRequestValidatorTests()
    {
        _sut = new UserLoginRequestValidator();
    }

    [Theory]
    [ClassData(typeof(UserLoginRequestValidatorWrongData))]
    public async Task ValidateUserLoginRequest_Should_Fail_WhenWrongUserLoginRequest(UserLoginRequest userLoginRequest)
    {
        // Act
        var result = await _sut.TestValidateAsync(userLoginRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [ClassData(typeof(UserLoginRequestValidatorCorrectData))]
    public async Task ValidateUserLoginRequest_Should_Success_WhenUserLoginRequest(UserLoginRequest userLoginRequest)
    {
        // Act
        var result = await _sut.TestValidateAsync(userLoginRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}