using CourseSystem.Persistence.Abstractions;

namespace CourseSystem.Infrastructure.Repositories;

public abstract class Repository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _dbContext;

    protected Repository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public virtual void Add(T entity) => _dbContext.Set<T>().Add(entity);
}
