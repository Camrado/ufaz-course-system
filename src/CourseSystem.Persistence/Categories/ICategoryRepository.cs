namespace CourseSystem.Persistence.Categories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Category?> GetByNameAsync(string title, CancellationToken cancellationToken = default);

    void Add(Category category, CancellationToken cancellationToken = default);

    void Delete(Category category, CancellationToken cancellationToken = default);
}
