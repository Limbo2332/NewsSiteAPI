using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.BLL.Services;

public class TagsService : BaseEntityService<Tag, TagResponse>, ITagsService
{
    private readonly ITagsRepository _tagsRepository;

    public TagsService(
        UserManager<IdentityUser> userManager,
        IMapper mapper,
        ITagsRepository tagsRepository)
        : base(userManager, mapper)
    {
        _tagsRepository = tagsRepository;
    }

    public async Task<PageList<TagResponse>> GetTagsAsync(PageSettings? pageSettings)
    {
        var tags = _tagsRepository.GetAll();

        var pageList = await GetAllAsync(tags, pageSettings);

        return pageList;
    }

    public async Task<TagResponse> GetTagByIdAsync(Guid id)
    {
        var tag = await _tagsRepository.GetByIdAsync(id)
                  ?? throw new NotFoundException(nameof(Tag), id);

        return _mapper.Map<TagResponse>(tag);
    }

    public async Task<TagResponse> AddTagForNewsIdAsync(NewsTagRequest newsTagRequest)
    {
        var newsTag = await _tagsRepository.AddTagForNewsIdAsync(newsTagRequest.TagId, newsTagRequest.NewsId)
                      ?? throw new NotFoundException(nameof(Tag),
                          newsTagRequest.TagId,
                          nameof(News),
                          newsTagRequest.NewsId);

        var tag = await GetTagByIdAsync(newsTag.TagId);

        return _mapper.Map<TagResponse>(tag);
    }

    public async Task<TagResponse> CreateNewTagAsync(NewTagRequest newTag)
    {
        var tag = _mapper.Map<Tag>(newTag);

        await _tagsRepository.AddAsync(tag);

        return _mapper.Map<TagResponse>(tag);
    }

    public async Task<TagResponse> UpdateTagAsync(UpdateTagRequest newTag)
    {
        var tag = _mapper.Map<Tag>(newTag);

        await _tagsRepository.UpdateAsync(tag);

        return _mapper.Map<TagResponse>(tag);
    }

    public async Task DeleteTagAsync(Guid id)
    {
        await _tagsRepository.DeleteAsync(id);
    }

    public async Task DeleteTagForNewsIdAsync(Guid tagId, Guid newsId)
    {
        await _tagsRepository.DeleteTagForNewsIdAsync(tagId, newsId);
    }

    protected override Expression<Func<Tag, bool>> GetFilteringExpressionFunc(string propertyName, string propertyValue)
    {
        return propertyName.ToLower() switch
        {
            "name" => tag => tag.Name.ToLower().Contains(propertyValue.ToLower()),
            _ => tag => true
        };
    }

    protected override Expression<Func<Tag, object>> GetSortingExpressionFunc(string sortingValue)
    {
        return sortingValue.ToLower() switch
        {
            "name" => tag => tag.Name,
            _ => tag => tag.UpdatedAt
        };
    }
}