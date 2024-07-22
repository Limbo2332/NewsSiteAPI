using AutoMapper;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;

namespace NewsSite.BLL.MappingProfiles;

public class TagsProfile : Profile
{
    public TagsProfile()
    {
        CreateMap<Tag, TagResponse>();
        CreateMap<NewTagRequest, Tag>();
        CreateMap<UpdateTagRequest, Tag>();
    }
}