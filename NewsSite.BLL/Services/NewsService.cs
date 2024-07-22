using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.BLL.Services;

public class NewsService : BaseEntityService<News, NewsResponse>, INewsService
{
    private readonly IAuthorsRepository _authorsRepository;
    private readonly INewsRepository _newsRepository;

    public NewsService(
        UserManager<IdentityUser> userManager,
        IMapper mapper,
        INewsRepository newsRepository,
        IAuthorsRepository authorsRepository)
        : base(userManager, mapper)
    {
        _newsRepository = newsRepository;
        _authorsRepository = authorsRepository;
    }

    public async Task<PageList<NewsResponse>> GetNewsAsync(PageSettings? pageSettings)
    {
        var news = _newsRepository.GetAll();

        var pageList = await GetAllAsync(news, pageSettings);

        return pageList;
    }

    public async Task<PageList<NewsResponse>> GetNewsByRubricAsync(Guid rubricId, PageSettings? pageSettings)
    {
        var news =
            _newsRepository.GetAll()
                .Include(n => n.NewsRubrics)
                .Where(n => n.NewsRubrics!.Any(nr => nr.RubricId == rubricId));

        var pageList = await GetAllAsync(news, pageSettings);

        return pageList;
    }

    public async Task<PageList<NewsResponse>> GetNewsByTagsAsync(List<Guid> tagsIds, PageSettings? pageSettings)
    {
        var news =
            _newsRepository.GetAll()
                .Include(n => n.NewsTags)
                .Where(n => n.NewsTags!.Any(nt => tagsIds.Contains(nt.TagId)));

        var pageList = await GetAllAsync(news, pageSettings);

        return pageList;
    }

    public async Task<PageList<NewsResponse>> GetNewsByAuthorAsync(Guid authorId, PageSettings? pageSettings)
    {
        var news =
            _newsRepository.GetAll()
                .Where(n => n.CreatedBy == authorId);

        var pageList = await GetAllAsync(news, pageSettings);

        return pageList;
    }

    public async Task<PageList<NewsResponse>> GetNewsByPeriodOfTimeAsync(DateTime startDate, DateTime endDate,
        PageSettings? pageSettings)
    {
        var news =
            _newsRepository.GetAll()
                .Where(n => n.UpdatedAt >= startDate && n.UpdatedAt <= endDate);

        var pageList = await GetAllAsync(news, pageSettings);

        return pageList;
    }

    public async Task<NewsResponse> GetNewsByIdAsync(Guid id)
    {
        var news = await _newsRepository.GetByIdAsync(id)
                   ?? throw new NotFoundException(nameof(News), id);

        return _mapper.Map<NewsResponse>(news);
    }

    public async Task<NewsResponse> CreateNewNewsAsync(NewNewsRequest newNewsRequest)
    {
        var newNews = _mapper.Map<News>(newNewsRequest);

        _ = await _authorsRepository.GetByIdAsync(newNewsRequest.AuthorId)
            ?? throw new NotFoundException(nameof(Author), newNewsRequest.AuthorId);

        await _newsRepository.AddAsync(newNews);

        if (newNewsRequest.RubricsIds is not null && newNewsRequest.RubricsIds.Any())
            await _newsRepository.AddNewsRubricsAsync(newNews.Id, newNewsRequest.RubricsIds);

        if (newNewsRequest.TagsIds is not null && newNewsRequest.TagsIds.Any())
            await _newsRepository.AddNewsTagsAsync(newNews.Id, newNewsRequest.TagsIds);

        return _mapper.Map<NewsResponse>(newNews);
    }

    public async Task<NewsResponse> UpdateNewsAsync(UpdateNewsRequest updateNewsRequest)
    {
        var updateNews = _mapper.Map<News>(updateNewsRequest);

        await _newsRepository.UpdateAsync(updateNews);

        return _mapper.Map<NewsResponse>(updateNews);
    }

    public async Task DeleteNewsAsync(Guid newsId)
    {
        await _newsRepository.DeleteAsync(newsId);
    }

    protected override Expression<Func<News, bool>> GetFilteringExpressionFunc(string propertyName,
        string propertyValue)
    {
        return propertyName.ToLower() switch
        {
            "content" => news => news.Content.ToLower().Contains(propertyValue.ToLower()),
            "subject" => news => news.Subject.ToLower().Contains(propertyValue.ToLower()),
            _ => news => true
        };
    }

    protected override Expression<Func<News, object>> GetSortingExpressionFunc(string sortingValue)
    {
        return sortingValue.ToLower() switch
        {
            "content" => news => news.Content,
            "subject" => news => news.Subject,
            _ => news => news.UpdatedAt
        };
    }
}