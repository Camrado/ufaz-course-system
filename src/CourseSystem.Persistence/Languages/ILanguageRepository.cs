namespace CourseSystem.Persistence.Languages;

public interface ILanguageRepository
{
    Task<IEnumerable<Language>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Language?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Language?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    void Add(Language language, CancellationToken cancellationToken = default);

    void Delete(Language language, CancellationToken cancellationToken = default);
}
