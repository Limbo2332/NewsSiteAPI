using NewsSite.DAL.DTO.Request.Author;

namespace NewsSite.UnitTests.TestData.Validators.Author.UpdatedAuthorRequestData;

public class UpdatedAuthorRequestValidatorWrongData : TheoryData<UpdatedAuthorRequest>
{
    public UpdatedAuthorRequestValidatorWrongData()
    {
        Add(new UpdatedAuthorRequest
        {
            Email = "WrongEmail",
            BirthDate = DateTime.MaxValue,
            FullName = "   wrong full name"
        });
        Add(new UpdatedAuthorRequest
        {
            Email = "wrong@email.",
            BirthDate = DateTime.UtcNow,
            FullName = string.Empty
        });
    }
}