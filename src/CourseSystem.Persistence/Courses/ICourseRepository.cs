namespace CourseSystem.Persistence.Courses;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Course?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameCategoryAndLanguageAsync(string name, int? categoryId, int? languageId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameCategoryAndLanguageExcludingIdAsync(int id, string name, int? categoryId, int? languageId,
        CancellationToken cancellationToken = default);

    void Add(Course course, CancellationToken cancellationToken = default);

    void Delete(Course course, CancellationToken cancellationToken = default);
}
