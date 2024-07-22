using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.Entities;

namespace NewsSite.DAL.Context.EntityConfigurations;

public class RubricConfig : IEntityTypeConfiguration<Rubric>
{
    public void Configure(EntityTypeBuilder<Rubric> builder)
    {
        builder
            .Property(r => r.Name)
            .HasMaxLength(ConfigurationConstants.RUBRIC_MAXLENGTH);

        builder
            .HasIndex(r => r.Name);
    }
}