using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NewsSite.DAL.Entities.Abstract;

namespace NewsSite.DAL.Interceptors;

public class UpdatedEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetCreatedAndUpdatedAtToEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        SetCreatedAndUpdatedAtToEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetCreatedAndUpdatedAtToEntities(DbContext? dbContext)
    {
        if (dbContext is null) return;

        var newEntries = dbContext.ChangeTracker.Entries<BaseEntity>()
            .Where(entryEntity => entryEntity.State == EntityState.Added)
            .Select(entryEntity => entryEntity.Entity);

        foreach (var newEntry in newEntries)
        {
            newEntry.CreatedAt = DateTime.UtcNow;
            newEntry.UpdatedAt = DateTime.UtcNow;
        }

        var updatedEntries = dbContext.ChangeTracker.Entries<BaseEntity>()
            .Where(entryEntity => entryEntity.State == EntityState.Modified)
            .Select(entryEntity => entryEntity.Entity);

        foreach (var updatedEntry in updatedEntries) updatedEntry.UpdatedAt = DateTime.UtcNow;
    }
}