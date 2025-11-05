using CourseSystem.Infrastructure;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace CourseSystem.Integration.Tests.Common;

[CollectionDefinition("Integration Tests", DisableParallelization = true)]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture> { }

public class IntegrationTestFixture : IAsyncLifetime
{
    private PostgreSqlContainer _postgres = null!;
    private ApiFactory _factory = null!;

    public HttpClient Client { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        // Create unique database for the run
        _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithDatabase($"ufaz_course_system_test_{Guid.NewGuid():N}")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();

        await _postgres.StartAsync();

        _factory = new ApiFactory(_postgres);
        Client = _factory.CreateDefaultClient(new Uri("http://localhost"));
        Client.Timeout = TimeSpan.FromSeconds(15);

        // Wait for DB readiness
        var retries = 0;
        while (retries < 10)
        {
            try
            {
                using var scopeCheck = _factory.Services.CreateScope();
                var db = scopeCheck.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await db.Database.OpenConnectionAsync();
                await db.Database.CloseConnectionAsync();
                break;
            }
            catch
            {
                retries++;
                await Task.Delay(1000);
            }
        }

        // Apply migrations
        await using var initScope = _factory.Services.CreateAsyncScope();
        var context = initScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        // Seed initial data
        await SeedDatabaseAsync(context);
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
        await _postgres.StopAsync();
        await _postgres.DisposeAsync();
    }

    public ApplicationDbContext CreateDbContext()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    private static async Task SeedDatabaseAsync(ApplicationDbContext context)
    {
        var seedPath = Path.Combine(AppContext.BaseDirectory, "Common", "Seed", "seedData.sql");
        if (File.Exists(seedPath))
        {
            var sql = await File.ReadAllTextAsync(seedPath);
            await context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}