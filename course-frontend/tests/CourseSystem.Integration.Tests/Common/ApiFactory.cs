using CourseSystem.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace CourseSystem.Integration.Tests.Common;

internal sealed class ApiFactory : WebApplicationFactory<Program>
{
    private readonly PostgreSqlContainer _postgres;

    public ApiFactory(PostgreSqlContainer postgres)
    {
        _postgres = postgres;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            // Remove the existing ApplicationDbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Re-register DbContext with Testcontainers connection string
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_postgres.GetConnectionString()));

            // Make sure controllers are visible to test host
            services.AddControllers()
                .AddApplicationPart(typeof(Program).Assembly);
        });
    }
}