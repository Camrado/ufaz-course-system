using CourseSystem.Persistence.Languages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseSystem.Infrastructure.Configurations.Languages;

public class LanguageConfiguration : SoftDeleteEntityConfiguration<Language>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("languages");

        builder.Property(e => e.Code).IsRequired();
    }
}
