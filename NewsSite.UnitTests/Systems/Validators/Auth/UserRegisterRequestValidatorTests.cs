using FluentValidation.TestHelper;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.Constants;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.UI.Validators.Auth;
using NewsSite.UnitTests.TestData.Validators.Auth.UserRegisterRequestData;

namespace NewsSite.UnitTests.Systems.Validators.Auth;

public class UserRegisterRequestValidatorTests
{
    private readonly IAuthorsService _authorsService;
    private readonly UserRegisterRequestValidator _sut;

    public UserRegisterRequestValidatorTests()
    {
        _authorsService = Substitute.For<IAuthorsService>();

        _sut = new UserRegisterRequestValidator(_authorsService);
    }

    [Theory]
    [ClassData(typeof(UserRegisterRequestValidatorWrongData))]
    public async Task ValidateUserRegisterRequest_Should_Fail_WhenWrongUserRegisterRequest(
        UserRegisterRequest userRegisterRequest)
    {
        // Act
        var result = await _sut.TestValidateAsync(userRegisterRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }

    [Theory]
    [ClassData(typeof(UserRegisterRequestValidatorCorrectData))]
    public async Task ValidateUserRegisterRequest_Should_Fail_WhenEmailAndFullNameAlreadyExists(
        UserRegisterRequest userRegisterRequest)
    {
        // Arrange
        _authorsService
            .IsEmailUnique(userRegisterRequest.Email)
            .Returns(false);

        _authorsService
            .IsFullNameUnique(userRegisterRequest.FullName)
            .Returns(false);

        // Act
        var result = await _sut.TestValidateAsync(userRegisterRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(
                ValidationMessages.GetEntityIsNotUniqueMessage(ValidationMessages.EMAIL_PROPERTY_NAME));
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage(
                ValidationMessages.GetEntityIsNotUniqueMessage(ValidationMessages.FULL_NAME_PROPERTY_NAME));
    }

    [Theory]
    [ClassData(typeof(UserRegisterRequestValidatorCorrectData))]
    public async Task ValidateUserRegisterRequest_Should_Success_WhenUserRegisterRequest(
        UserRegisterRequest userRegisterRequest)
    {
        // Arrange
        _authorsService
            .IsEmailUnique(userRegisterRequest.Email)
            .Returns(true);

        _authorsService
            .IsFullNameUnique(userRegisterRequest.FullName)
            .Returns(true);

        // Act
        var result = await _sut.TestValidateAsync(userRegisterRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.FullName);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
        result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);
        result.ShouldNotHaveValidationErrorFor(x => x.PublicInformation);
        result.ShouldNotHaveValidationErrorFor(x => x.Sex);
    }
}