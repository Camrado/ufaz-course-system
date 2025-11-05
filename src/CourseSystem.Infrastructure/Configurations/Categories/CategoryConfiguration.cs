using CourseSystem.Persistence.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseSystem.Infrastructure.Configurations.Categories;

internal sealed class CategoryConfiguration : SoftDeleteEntityConfiguration<Category>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.Property(c => c.Name)
            .IsRequired();

        // builder.HasIndex(c => c.Name)
            // .IsUnique()
            // .HasFilter("\"deleted_at\" IS NULL");
    }
}
