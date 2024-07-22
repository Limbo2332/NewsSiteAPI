using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.Entities;

namespace NewsSite.BLL.MappingProfiles.Resolvers;

public class IdentityUserResolver
    : IValueResolver<UserRegisterRequest, Author, IdentityUser>,
        IValueResolver<UserLoginRequest, Author, IdentityUser>,
        IValueResolver<UpdatedAuthorRequest, Author, IdentityUser>
{
    private readonly UserManager<IdentityUser> _userManager;

    public IdentityUserResolver(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public IdentityUser Resolve(UpdatedAuthorRequest source, Author destination, IdentityUser destMember,
        ResolutionContext context)
    {
        return _userManager.FindByEmailAsync(source.Email).Result!;
    }

    public IdentityUser Resolve(UserLoginRequest source, Author destination, IdentityUser destMember,
        ResolutionContext context)
    {
        return _userManager.FindByEmailAsync(source.Email).Result!;
    }

    public IdentityUser Resolve(UserRegisterRequest source, Author destination, IdentityUser destMember,
        ResolutionContext context)
    {
        return _userManager.FindByEmailAsync(source.Email).Result!;
    }
}