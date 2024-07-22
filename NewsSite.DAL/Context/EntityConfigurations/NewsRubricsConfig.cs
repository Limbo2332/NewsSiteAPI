using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsSite.DAL.Entities;

namespace NewsSite.DAL.Context.EntityConfigurations;

public class NewsRubricsConfig : IEntityTypeConfiguration<NewsRubrics>
{
    public void Configure(EntityTypeBuilder<NewsRubrics> builder)
    {
        builder
            .HasKey(nr => new { nr.NewsId, nr.RubricId });
    }
}