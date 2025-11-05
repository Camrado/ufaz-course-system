using CourseSystem.Persistence.Categories;
using Microsoft.EntityFrameworkCore;

namespace CourseSystem.Infrastructure.Repositories;

public sealed class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower(), cancellationToken);

    public void Add(Category category, CancellationToken cancellationToken = default) =>
        _dbContext.Categories.Add(category);

    public void Delete(Category category, CancellationToken cancellationToken = default) =>
        _dbContext.Categories.Remove(category);
}
