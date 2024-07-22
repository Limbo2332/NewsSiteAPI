using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.BLL.Interfaces;

public interface IAuthorsService
{
    Task<PageList<AuthorResponse>> GetAuthorsAsync(PageSettings? pageSettings);

    Task<AuthorResponse> GetAuthorByIdAsync(Guid authorId);

    Task<AuthorResponse> UpdateAuthorAsync(UpdatedAuthorRequest updatedAuthor);

    Task DeleteAuthorAsync(Guid authorId);

    bool IsEmailUnique(string email);

    bool IsFullNameUnique(string fullName);
}