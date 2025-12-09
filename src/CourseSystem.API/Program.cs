using CourseSystem.API.Extensions;
using CourseSystem.Application;
using CourseSystem.Infrastructure;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();

builder.Services.AddExceptionHandling();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

var test = 12;

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
