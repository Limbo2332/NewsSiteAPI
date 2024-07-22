using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.DTO.Request.Author;

namespace NewsSite.UnitTests.TestData.Validators.Author.UpdatedAuthorRequestData;

public class UpdatedAuthorRequestValidatorCorrectData : TheoryData<UpdatedAuthorRequest>
{
    public UpdatedAuthorRequestValidatorCorrectData()
    {
        Add(new UpdatedAuthorRequest
        {
            BirthDate = DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER * 2),
            Email = "correct@email.com",
            FullName = "correctFullName",
            PublicInformation = null
        });
        Add(new UpdatedAuthorRequest
        {
            BirthDate = DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER * 3),
            Email = "correct@ukr.net",
            FullName = "correct FullName",
            PublicInformation = "publicInformation"
        });
    }
}