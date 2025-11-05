using CourseSystem.Persistence.Languages;
using Microsoft.EntityFrameworkCore;

namespace CourseSystem.Infrastructure.Repositories;

public class LanguageRepository : Repository<Language>, ILanguageRepository
{
    public LanguageRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Language>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Languages
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Language?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _dbContext.Languages
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

    public async Task<Language?> GetByCodeAsync(string code, CancellationToken cancellationToken = default) =>
        await _dbContext.Languages
            .FirstOrDefaultAsync(l => l.Code.ToLower() == code.ToLower(), cancellationToken);

    public void Add(Language language, CancellationToken cancellationToken = default) =>
        _dbContext.Languages.Add(language);

    public void Delete(Language language, CancellationToken cancellationToken = default) =>
        _dbContext.Languages.Remove(language);
}
