using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.Entities;

namespace NewsSite.DAL.Context.EntityConfigurations;

public class AuthorConfig : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder
            .Property(a => a.FullName)
            .HasMaxLength(ConfigurationConstants.FULL_NAME_MAXLENGTH);

        builder
            .Property(a => a.Email)
            .HasMaxLength(ConfigurationConstants.EMAIL_MAXLENGTH);

        builder
            .Property(a => a.PublicInformation)
            .HasMaxLength(ConfigurationConstants.PUBLIC_INFORMATION_MAXLENGTH);

        builder
            .HasMany(a => a.News)
            .WithOne(pon => pon.Author)
            .HasForeignKey(pon => pon.CreatedBy)
            .IsRequired();
    }
}