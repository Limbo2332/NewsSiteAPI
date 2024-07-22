using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.DAL.Repositories;

public class RubricsRepository : GenericRepository<Rubric>, IRubricsRepository
{
    private readonly INewsRepository _newsRepository;

    public RubricsRepository(
        OnlineNewsContext context,
        INewsRepository newsRepository)
        : base(context)
    {
        _newsRepository = newsRepository;
    }

    public async Task<NewsRubrics?> AddRubricForNewsIdAsync(Guid rubricId, Guid newsId)
    {
        var rubric = await GetByIdAsync(rubricId);
        var news = await _newsRepository.GetByIdAsync(newsId);
        var newsRubrics = await GetNewsRubricsAsync(rubricId, newsId);

        if (rubric is null || news is null || newsRubrics is not null) return null;

        var newNewsRubrics = new NewsRubrics
        {
            RubricId = rubricId,
            NewsId = newsId
        };

        await _context.NewsRubrics.AddAsync(newNewsRubrics);
        await _context.SaveChangesAsync();

        return newNewsRubrics;
    }

    public async Task DeleteRubricForNewsIdAsync(Guid rubricId, Guid newsId)
    {
        var newsRubrics = await GetNewsRubricsAsync(rubricId, newsId);

        if (newsRubrics is not null)
        {
            _context.NewsRubrics.Remove(newsRubrics);
            await _context.SaveChangesAsync();
        }
    }

    private async Task<NewsRubrics?> GetNewsRubricsAsync(Guid rubricId, Guid newsId)
    {
        return await _context.NewsRubrics
            .FirstOrDefaultAsync(nt => nt.RubricId == rubricId && nt.NewsId == newsId);
    }
}