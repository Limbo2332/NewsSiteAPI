using System.Globalization;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Extensions;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.BLL.Services;

public class AuthorsService : BaseEntityService<Author, AuthorResponse>, IAuthorsService
{
    private readonly IAuthorsRepository _authorsRepository;

    public AuthorsService(
        UserManager<IdentityUser> userManager,
        IMapper mapper,
        IAuthorsRepository authorsRepository)
        : base(userManager, mapper)
    {
        _authorsRepository = authorsRepository;
    }

    public async Task<PageList<AuthorResponse>> GetAuthorsAsync(PageSettings? pageSettings)
    {
        var authors = _authorsRepository.GetAll();

        var pageList = await GetAllAsync(authors, pageSettings);

        return pageList;
    }

    public async Task<AuthorResponse> GetAuthorByIdAsync(Guid authorId)
    {
        var author = await GetAuthorEntityByIdAsync(authorId);

        return _mapper.Map<AuthorResponse>(author);
    }

    public async Task<AuthorResponse> UpdateAuthorAsync(UpdatedAuthorRequest updatedAuthor)
    {
        var authorToUpdate = await GetAuthorEntityByIdAsync(updatedAuthor.Id);

        if (authorToUpdate.Email != updatedAuthor.Email)
        {
            await _userManager.SetEmailAsync(authorToUpdate.IdentityUser, updatedAuthor.Email);
            authorToUpdate.Email = updatedAuthor.Email;
        }

        if (authorToUpdate.FullName != updatedAuthor.FullName)
        {
            await _userManager.SetUserNameAsync(authorToUpdate.IdentityUser, updatedAuthor.FullName);
            authorToUpdate.FullName = updatedAuthor.FullName;
        }

        authorToUpdate.Sex = updatedAuthor.Sex;
        authorToUpdate.BirthDate = updatedAuthor.BirthDate;
        authorToUpdate.PublicInformation = updatedAuthor.PublicInformation;

        await _authorsRepository.UpdateAsync(authorToUpdate);
        return _mapper.Map<AuthorResponse>(authorToUpdate);
    }

    public async Task DeleteAuthorAsync(Guid authorId)
    {
        await _authorsRepository.DeleteAsync(authorId);
    }

    public bool IsEmailUnique(string email)
    {
        return _authorsRepository.IsEmailUnique(email);
    }

    public bool IsFullNameUnique(string fullName)
    {
        return _authorsRepository.IsFullNameUnique(fullName);
    }

    protected override Expression<Func<Author, bool>> GetFilteringExpressionFunc(string propertyName,
        string propertyValue)
    {
        return propertyName.ToLower() switch
        {
            "email" => author => author.Email.ToLower().Contains(propertyValue.ToLower()),
            "fullname" => author => author.FullName.ToLower().Contains(propertyValue.ToLower()),
            "birthdate" => author => propertyValue.IsDateTime()
                                     && author.BirthDate >=
                                     Convert.ToDateTime(propertyValue, CultureInfo.InvariantCulture),
            "publicinformation" => author => author.PublicInformation != null
                                             && author.PublicInformation.ToLower().Contains(propertyValue.ToLower()),
            _ => author => true
        };
    }

    protected override Expression<Func<Author, object>> GetSortingExpressionFunc(string sortingValue)
    {
        return sortingValue.ToLower() switch
        {
            "email" => author => author.Email,
            "fullname" => author => author.FullName,
            "birthdate" => author => author.BirthDate,
            "publicinformation" => author => author.PublicInformation != null
                ? author.PublicInformation
                : 0,
            _ => author => author.UpdatedAt
        };
    }

    private async Task<Author> GetAuthorEntityByIdAsync(Guid id)
    {
        return await _authorsRepository.GetByIdAsync(id)
               ?? throw new NotFoundException(nameof(Author), id);
    }
}