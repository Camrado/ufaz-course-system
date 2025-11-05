using System.Net;
using System.Net.Http.Json;
using CourseSystem.Application.Categories.CreateCategory;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Infrastructure;
using CourseSystem.Integration.Tests.Common;
using CourseSystem.Integration.Tests.Common.Response;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CourseSystem.Integration.Tests.Categories;

public class CreateCategoryCommandTests : IClassFixture<IntegrationTestFixture>
{
    private readonly HttpClient _client;
    private readonly IntegrationTestFixture _fixture;

    public CreateCategoryCommandTests(IntegrationTestFixture fixture)
    {
        _client = fixture.Client;
        _fixture = fixture;
    }

    [Theory]
    [InlineData("Programming")]
    public async Task Should_Create_Category_When_ValidDataProvided(string categoryName)
    {
        var command = new CreateCategoryCommand(categoryName);
        var response = await _client.PostAsJsonAsync("/api/categories", command);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadFromJsonAsync<CreateCategoryCommandResponse>();
        content.Should().NotBeNull();

        await using var dbContext = _fixture.CreateDbContext();
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
        category.Should().NotBeNull();
    }

    [Theory]
    [InlineData(CategoryConstants.EmptyCategoryName)]
    public async Task Should_Return_BadRequest_When_CategoryNameIsEmpty(string categoryName)
    {
        var command = new CreateCategoryCommand(categoryName);
        var response = await _client.PostAsJsonAsync("/api/categories", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ValidationException));
        content.Detail.Should().Be("One or more validation errors occurred.");
        content.Status.Should().Be((int)HttpStatusCode.BadRequest);
        content.Errors.Should().ContainKey("Name");
        content.Errors["Name"].Should().Contain("Name is required.");
    }

    [Theory]
    [InlineData(CategoryConstants.SampleCategoryName)]
    public async Task Should_Return_Conflict_When_CategoryNameAlreadyExists(string categoryName)
    {
        var command = new CreateCategoryCommand(categoryName);
        var response = await _client.PostAsJsonAsync("/api/categories", command);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ConflictException));
        content.Detail.Should().Be($"Category with title '{categoryName}' already exists.");
        content.Status.Should().Be((int)HttpStatusCode.Conflict);
    }
}
