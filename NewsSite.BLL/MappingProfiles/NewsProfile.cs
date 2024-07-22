using AutoMapper;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;

namespace NewsSite.BLL.MappingProfiles;

public class NewsProfile : Profile
{
    public NewsProfile()
    {
        CreateMap<News, NewsResponse>()
            .ForMember(dest => dest.AuthorId, src => src.MapFrom(n => n.CreatedBy));

        CreateMap<NewNewsRequest, News>()
            .ForMember(dest => dest.CreatedBy, src => src.MapFrom(n => n.AuthorId));

        CreateMap<UpdateNewsRequest, News>()
            .ForMember(dest => dest.CreatedBy, src => src.MapFrom(n => n.AuthorId));
        ;
    }
}