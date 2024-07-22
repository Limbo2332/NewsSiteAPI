using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.DAL.Repositories;

public class AuthorsRepository : GenericRepository<Author>, IAuthorsRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    public AuthorsRepository(
        OnlineNewsContext context,
        UserManager<IdentityUser> userManager)
        : base(context)
    {
        _userManager = userManager;
    }

    public override async Task<Author?> GetByIdAsync(Guid id)
    {
        var author = await _dbSet.FirstOrDefaultAsync(a => a.Id == id);

        if (author is null) return null;

        var normalizedEmail = _userManager.NormalizeEmail(author.Email);
        var identityUser = await _userManager.FindByEmailAsync(normalizedEmail);

        if (identityUser is not null) author.IdentityUser = identityUser;

        return author;
    }

    public async Task<Author?> GetAuthorByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.Email == email);
    }

    public bool IsEmailUnique(string email)
    {
        return !_dbSet.Any(a => a.Email == email);
    }

    public bool IsFullNameUnique(string fullName)
    {
        return !_dbSet.Any(a => a.FullName == fullName);
    }

    public override async Task DeleteAsync(Guid id)
    {
        var author = await GetByIdAsync(id);

        if (author is not null)
        {
            await _userManager.DeleteAsync(author.IdentityUser);

            _dbSet.Remove(author);
            await _context.SaveChangesAsync();
        }
    }
}