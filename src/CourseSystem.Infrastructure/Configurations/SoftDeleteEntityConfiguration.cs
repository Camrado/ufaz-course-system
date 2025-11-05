using CourseSystem.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseSystem.Infrastructure.Configurations;

public abstract class SoftDeleteEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasQueryFilter(e => e.DeletedAt == null);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.DeletedAt)
            .IsRequired(false);

        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.DeletedBy)
            .IsRequired(false);

        builder.Property(e => e.UpdatedBy)
            .IsRequired(false);

        builder.Property(e => e.CreatedBy)
            .IsRequired(false);

        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
}
