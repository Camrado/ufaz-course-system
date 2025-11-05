using CourseSystem.Persistence.Courses;
using Microsoft.EntityFrameworkCore;

namespace CourseSystem.Infrastructure.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Courses
            .AsNoTracking()
            .Include(c => c.Category)
            .Include(c => c.Language)
            .ToListAsync(cancellationToken);
    }

    public async Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<Course?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default) =>
        await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Slug!.ToLower() == slug.ToLower(), cancellationToken);

    public async Task<bool> ExistsByNameCategoryAndLanguageAsync(string name, int? categoryId, int? languageId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Courses
            .AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.CategoryId == categoryId && c.LanguageId == languageId,
                cancellationToken);
    }

    public async Task<bool> ExistsByNameCategoryAndLanguageExcludingIdAsync(int id, string name, int? categoryId, int? languageId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Courses
            .AnyAsync(
                c => c.Id != id && c.Name.ToLower() == name.ToLower() && c.CategoryId == categoryId &&
                     c.LanguageId == languageId,
                cancellationToken);
    }

    public void Add(Course course, CancellationToken cancellationToken = default) =>
        _dbContext.Courses.Add(course);

    public void Delete(Course course, CancellationToken cancellationToken = default) =>
        _dbContext.Courses.Remove(course);
}
