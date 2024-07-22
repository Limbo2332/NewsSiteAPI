using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsSite.DAL.Entities;

namespace NewsSite.DAL.Context.EntityConfigurations;

public class NewsTagsConfig : IEntityTypeConfiguration<NewsTags>
{
    public void Configure(EntityTypeBuilder<NewsTags> builder)
    {
        builder
            .HasKey(nt => new { nt.NewsId, nt.TagId });
    }
}