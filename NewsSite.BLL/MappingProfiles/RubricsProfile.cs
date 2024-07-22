using AutoMapper;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;

namespace NewsSite.BLL.MappingProfiles;

public class RubricsProfile : Profile
{
    public RubricsProfile()
    {
        CreateMap<Rubric, RubricResponse>();
        CreateMap<NewRubricRequest, Rubric>();
        CreateMap<UpdateRubricRequest, Rubric>();
    }
}