using FluentValidation.TestHelper;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.UI.Validators.Author;
using NewsSite.UnitTests.TestData.Validators.Author.UpdatedAuthorRequestData;

namespace NewsSite.UnitTests.Systems.Validators.Author;

public class UpdatedAuthorRequestValidatorTests
{
    private readonly IAuthorsService _authorsService;
    private readonly UpdatedAuthorRequestValidator _sut;

    public UpdatedAuthorRequestValidatorTests()
    {
        _authorsService = Substitute.For<IAuthorsService>();

        _sut = new UpdatedAuthorRequestValidator(_authorsService);
    }

    [Theory]
    [ClassData(typeof(UpdatedAuthorRequestValidatorWrongData))]
    public async Task ValidateUpdatedAuthorRequest_Should_Fail_WhenWrongUpdatedAuthorRequest(
        UpdatedAuthorRequest updatedAuthorRequest)
    {
        // Act
        var result = await _sut.TestValidateAsync(updatedAuthorRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }

    [Theory]
    [ClassData(typeof(UpdatedAuthorRequestValidatorCorrectData))]
    public async Task ValidateUpdatedAuthorRequest_Should_Success_WhenUpdatedAuthorRequest(
        UpdatedAuthorRequest updatedAuthorRequest)
    {
        // Arrange
        _authorsService
            .IsEmailUnique(updatedAuthorRequest.Email)
            .Returns(true);

        _authorsService
            .IsFullNameUnique(updatedAuthorRequest.FullName)
            .Returns(true);

        // Act
        var result = await _sut.TestValidateAsync(updatedAuthorRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.FullName);
        result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);
        result.ShouldNotHaveValidationErrorFor(x => x.PublicInformation);
        result.ShouldNotHaveValidationErrorFor(x => x.Sex);
    }
}