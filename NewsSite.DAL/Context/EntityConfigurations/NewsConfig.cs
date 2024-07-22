using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.Entities;

namespace NewsSite.DAL.Context.EntityConfigurations;

public class NewsConfig : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder
            .Property(pon => pon.Subject)
            .HasMaxLength(ConfigurationConstants.SUBJECT_MAXLENGTH);

        builder
            .Property(pon => pon.Content)
            .HasMaxLength(ConfigurationConstants.CONTENT_MAXLENGTH);

        builder
            .HasMany(pon => pon.NewsTags)
            .WithOne(nt => nt.News)
            .HasForeignKey(nt => nt.NewsId)
            .IsRequired();

        builder
            .HasMany(pon => pon.NewsRubrics)
            .WithOne(nt => nt.News)
            .HasForeignKey(nt => nt.NewsId)
            .IsRequired();
    }
}