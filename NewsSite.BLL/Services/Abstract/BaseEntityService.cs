using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.DTO.Response.Abstract;
using NewsSite.DAL.Entities.Abstract;

namespace NewsSite.BLL.Services.Abstract;

public abstract class BaseEntityService<TEntry, TResult> : BaseService
    where TEntry : BaseEntity
    where TResult : BaseResponse
{
    protected BaseEntityService(UserManager<IdentityUser> userManager, IMapper mapper) : base(userManager, mapper)
    {
    }

    protected abstract Expression<Func<TEntry, bool>> GetFilteringExpressionFunc(string propertyName,
        string propertyValue);

    protected abstract Expression<Func<TEntry, object>> GetSortingExpressionFunc(string sortingValue);

    public async Task<PageList<TResult>> GetAllAsync(IQueryable<TEntry> items, PageSettings? pageSettings)
    {
        if (pageSettings?.PageFiltering is not null)
        {
            var propertyValue = pageSettings.PageFiltering.PropertyValue.ToLower();

            var filteringFunc =
                GetFilteringExpressionFunc(pageSettings.PageFiltering.PropertyName, propertyValue);

            items = items.Where(filteringFunc);
        }

        if (pageSettings?.PageSorting is not null)
        {
            var sortingExpression = GetSortingExpressionFunc(pageSettings.PageSorting.SortingProperty);

            items = pageSettings.PageSorting.SortingOrder switch
            {
                SortingOrder.Ascending => items.OrderBy(sortingExpression),
                SortingOrder.Descending => items.OrderByDescending(sortingExpression),
                _ => items
            };
        }

        var totalItemsCount = items.Count();

        if (pageSettings?.PagePagination is not null)
            items = items
                .Skip(pageSettings.PagePagination.PageSize * (pageSettings.PagePagination.PageNumber - 1))
                .Take(pageSettings.PagePagination.PageSize);

        var itemsEnumerable = await items.ToListAsync();

        return new PageList<TResult>
        {
            TotalCount = totalItemsCount,
            PageSize = pageSettings?.PagePagination?.PageSize ?? PageList<AuthorResponse>.DefaultPageSize,
            PageNumber = pageSettings?.PagePagination?.PageNumber ?? 1,
            Items = _mapper.Map<List<TResult>>(itemsEnumerable)
        };
    }
}