using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.BLL.Services;

public class RubricsService : BaseEntityService<Rubric, RubricResponse>, IRubricsService
{
    private readonly IRubricsRepository _rubricsRepository;

    public RubricsService(
        UserManager<IdentityUser> userManager,
        IMapper mapper,
        IRubricsRepository rubricsRepository)
        : base(userManager, mapper)
    {
        _rubricsRepository = rubricsRepository;
    }

    public async Task<PageList<RubricResponse>> GetRubricsAsync(PageSettings? pageSettings)
    {
        var rubrics = _rubricsRepository.GetAll();

        var pageList = await GetAllAsync(rubrics, pageSettings);

        return pageList;
    }

    public async Task<RubricResponse> GetRubricByIdAsync(Guid id)
    {
        var rubric = await _rubricsRepository.GetByIdAsync(id)
                     ?? throw new NotFoundException(nameof(Rubric), id);

        return _mapper.Map<RubricResponse>(rubric);
    }

    public async Task<RubricResponse> AddRubricForNewsIdAsync(NewsRubricRequest newsRubricRequest)
    {
        var newsRubric =
            await _rubricsRepository.AddRubricForNewsIdAsync(newsRubricRequest.RubricId, newsRubricRequest.NewsId)
            ?? throw new NotFoundException(nameof(Rubric),
                newsRubricRequest.RubricId,
                nameof(News),
                newsRubricRequest.NewsId);

        var rubric = await GetRubricByIdAsync(newsRubric.RubricId);

        return _mapper.Map<RubricResponse>(rubric);
    }

    public async Task<RubricResponse> CreateNewRubricAsync(NewRubricRequest newRubric)
    {
        var rubric = _mapper.Map<Rubric>(newRubric);

        await _rubricsRepository.AddAsync(rubric);

        return _mapper.Map<RubricResponse>(rubric);
    }

    public async Task<RubricResponse> UpdateRubricAsync(UpdateRubricRequest newRubric)
    {
        var rubric = _mapper.Map<Rubric>(newRubric);

        await _rubricsRepository.UpdateAsync(rubric);

        return _mapper.Map<RubricResponse>(rubric);
    }

    public async Task DeleteRubricAsync(Guid id)
    {
        await _rubricsRepository.DeleteAsync(id);
    }

    public async Task DeleteRubricForNewsIdAsync(Guid rubricId, Guid newsId)
    {
        await _rubricsRepository.DeleteRubricForNewsIdAsync(rubricId, newsId);
    }

    protected override Expression<Func<Rubric, bool>> GetFilteringExpressionFunc(string propertyName,
        string propertyValue)
    {
        return propertyName.ToLower() switch
        {
            "name" => rubric => rubric.Name.ToLower().Contains(propertyValue.ToLower()),
            _ => rubric => true
        };
    }

    protected override Expression<Func<Rubric, object>> GetSortingExpressionFunc(string sortingValue)
    {
        return sortingValue.ToLower() switch
        {
            "name" => rubric => rubric.Name,
            _ => rubric => rubric.UpdatedAt
        };
    }
}