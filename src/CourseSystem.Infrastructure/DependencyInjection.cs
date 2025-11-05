using CourseSystem.Application.Abstractions.Localization;
using CourseSystem.Infrastructure.Extensions;
using CourseSystem.Infrastructure.Localization;
using CourseSystem.Infrastructure.Repositories;
using CourseSystem.Persistence.Abstractions;
using CourseSystem.Persistence.Categories;
using CourseSystem.Persistence.Courses;
using CourseSystem.Persistence.Languages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ILanguageCodeProvider, LanguageCodeProvider>();

        AddPersistence(services, configuration);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database") ??
                                  throw new ArgumentNullException(nameof(configuration),
                                      "Database connection string is missing.");

        services.AddApplicationDbContext(connectionString);

        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<ILanguageRepository, LanguageRepository>();

        services.AddScoped<ICourseRepository, CourseRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }
}
