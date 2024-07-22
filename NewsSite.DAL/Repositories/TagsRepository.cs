using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.DAL.Repositories;

public class TagsRepository : GenericRepository<Tag>, ITagsRepository
{
    private readonly INewsRepository _newsRepository;

    public TagsRepository(
        OnlineNewsContext context,
        INewsRepository newsRepository)
        : base(context)
    {
        _newsRepository = newsRepository;
    }

    public async Task<NewsTags?> AddTagForNewsIdAsync(Guid tagId, Guid newsId)
    {
        var tag = await GetByIdAsync(tagId);
        var news = await _newsRepository.GetByIdAsync(newsId);
        var newsTags = await GetNewsTagsAsync(tagId, newsId);

        if (tag is null || news is null || newsTags is not null) return null;

        var newNewsTags = new NewsTags
        {
            TagId = tagId,
            NewsId = newsId
        };

        await _context.NewsTags.AddAsync(newNewsTags);
        await _context.SaveChangesAsync();

        return newNewsTags;
    }

    public async Task DeleteTagForNewsIdAsync(Guid tagId, Guid newsId)
    {
        var newsTags = await GetNewsTagsAsync(tagId, newsId);

        if (newsTags is not null)
        {
            _context.NewsTags.Remove(newsTags);
            await _context.SaveChangesAsync();
        }
    }

    private async Task<NewsTags?> GetNewsTagsAsync(Guid tagId, Guid newsId)
    {
        return await _context.NewsTags
            .FirstOrDefaultAsync(nt => nt.TagId == tagId && nt.NewsId == newsId);
    }
}