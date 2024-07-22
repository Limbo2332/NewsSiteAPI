using AutoMapper;
using NewsSite.BLL.MappingProfiles.Resolvers;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;

namespace NewsSite.BLL.MappingProfiles;

public class AuthorsProfile : Profile
{
    public AuthorsProfile()
    {
        CreateMap<UserRegisterRequest, Author>()
            .ForMember(dest => dest.IdentityUser, src => src.MapFrom<IdentityUserResolver>());

        CreateMap<UserLoginRequest, Author>()
            .ForMember(dest => dest.IdentityUser, src => src.MapFrom<IdentityUserResolver>());

        CreateMap<Author, NewUserResponse>();
        CreateMap<Author, LoginUserResponse>();

        CreateMap<Author, AuthorResponse>();
        CreateMap<UpdatedAuthorRequest, Author>()
            .ForMember(dest => dest.IdentityUser, src => src.MapFrom<IdentityUserResolver>());
    }
}