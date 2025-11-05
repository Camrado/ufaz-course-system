namespace CourseSystem.Persistence.Abstractions;

public abstract class BaseEntity
{
    public int Id { get; protected init; }

    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string? CreatedBy { get; protected set; } = null;

    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; protected set; } = null;

    public DateTime? DeletedAt { get; protected set; } = null;
    public string? DeletedBy { get; protected set; } = null;

    public void MarkAsUpdated() => UpdatedAt = DateTime.UtcNow;

    public void MarkAsDeleted() => DeletedAt = DateTime.UtcNow;
}
