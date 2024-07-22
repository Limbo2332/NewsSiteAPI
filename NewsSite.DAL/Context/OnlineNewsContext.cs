using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context.EntityConfigurations;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Interceptors;

namespace NewsSite.DAL.Context;

public class OnlineNewsContext : IdentityDbContext
{
    private readonly UpdatedEntityInterceptor _updatedEntityInterceptor;

    public OnlineNewsContext(
        DbContextOptions<OnlineNewsContext> options,
        UpdatedEntityInterceptor updatedEntityInterceptor)
        : base(options)
    {
        _updatedEntityInterceptor = updatedEntityInterceptor;
    }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<News> News => Set<News>();
    public DbSet<Rubric> Rubrics => Set<Rubric>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<NewsRubrics> NewsRubrics => Set<NewsRubrics>();
    public DbSet<NewsTags> NewsTags => Set<NewsTags>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AuthorConfig).Assembly);

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_updatedEntityInterceptor);

        base.OnConfiguring(optionsBuilder);
    }
}