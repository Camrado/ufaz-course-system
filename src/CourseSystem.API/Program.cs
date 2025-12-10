using Microsoft.EntityFrameworkCore;
using CourseSystem.API.Extensions;
using CourseSystem.Application;
using CourseSystem.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CourseSystem API",
        Version = "v1",
        Description = "API documentation for the CourseSystem application."
    });
});

builder.Services.AddExceptionHandling();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();   // Apply migrations automatically
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CourseSystem API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseExceptionHandler();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html"); // SPA routing

app.MapControllers();

app.Run();
