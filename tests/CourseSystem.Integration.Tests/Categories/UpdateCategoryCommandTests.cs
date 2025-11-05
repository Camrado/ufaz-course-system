using System.Net;
using System.Net.Http.Json;
using CourseSystem.Application.Categories.UpdateCategory;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Infrastructure;
using CourseSystem.Integration.Tests.Common;
using CourseSystem.Integration.Tests.Common.Response;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CourseSystem.Integration.Tests.Categories;

public class UpdateCategoryCommandTests : IClassFixture<IntegrationTestFixture>
{
    private readonly HttpClient _client;
    private readonly IntegrationTestFixture _fixture;

    public UpdateCategoryCommandTests(IntegrationTestFixture fixture)
    {
        _client = fixture.Client;
        _fixture = fixture;
    }

    [Theory]
    [InlineData(CategoryConstants.SampleCategoryId, "Programming")]
    public async Task Should_Update_Category_When_ValidDataProvided(int categoryId, string updatedCategoryName)
    {
        var command = new UpdateCategoryCommand { CategoryId = categoryId, Name = updatedCategoryName };
        var response = await _client.PutAsJsonAsync($"/api/categories/{categoryId}", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<UpdateCategoryCommandResponse>();
        content.Should().NotBeNull();
        content.Id.Should().Be(categoryId);

        await using var dbContext = _fixture.CreateDbContext();
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        category.Should().NotBeNull();
        category.Name.Should().Be(updatedCategoryName);
    }

    [Theory]
    [InlineData(CategoryConstants.EmptyCategoryId, "Advanced Programming")]
    public async Task Should_Return_BadRequest_When_CategoryIdIsEmpty(int categoryId, string updatedCategoryName)
    {
        var command = new UpdateCategoryCommand { CategoryId = categoryId, Name = updatedCategoryName };
        var response = await _client.PutAsJsonAsync($"/api/categories/{categoryId}", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ValidationException));
        content.Detail.Should().Be("One or more validation errors occurred.");
        content.Status.Should().Be((int)HttpStatusCode.BadRequest);
        content.Errors.Should().ContainKey("CategoryId");
        content.Errors["CategoryId"].Should().Contain("Category ID must not be empty.");
    }

    [Theory]
    [InlineData(CategoryConstants.NonExistentCategoryId, "C# Programming")]
    public async Task Should_Return_NotFound_When_CategoryDoesNotExist(int categoryId, string updatedCategoryName)
    {
        var command = new UpdateCategoryCommand { CategoryId = categoryId, Name = updatedCategoryName };
        var response = await _client.PutAsJsonAsync($"/api/categories/{categoryId}", command);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(NotFoundException));
        content.Detail.Should().Be($"Category with ID '{categoryId}' not found.");
        content.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(CategoryConstants.SampleCategoryId, CategoryConstants.EmptyCategoryName)]
    public async Task Should_Return_BadRequest_When_CategoryNameIsEmpty(int categoryId, string updatedCategoryName)
    {
        var command = new UpdateCategoryCommand { CategoryId = categoryId, Name = updatedCategoryName };
        var response = await _client.PutAsJsonAsync($"/api/categories/{categoryId}", command);

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
    [InlineData(CategoryConstants.SampleCategoryId, CategoryConstants.ExistingCategoryName)]
    public async Task Should_Return_Conflict_When_CategoryNameAlreadyExists(int categoryId, string updatedCategoryName)
    {
        var command = new UpdateCategoryCommand { CategoryId = categoryId, Name = updatedCategoryName };
        var response = await _client.PutAsJsonAsync($"/api/categories/{categoryId}", command);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ConflictException));
        content.Detail.Should().Be($"Category with title '{updatedCategoryName}' already exists.");
        content.Status.Should().Be((int)HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData(CategoryConstants.NotUpdatedCategoryId, CategoryConstants.NotUpdatedCategoryName)]
    public async Task Should_Not_Throw_Exception_When_NoChangesMade(int categoryId, string categoryName)
    {
        var command = new UpdateCategoryCommand { CategoryId = categoryId, Name = categoryName };
        var response = await _client.PutAsJsonAsync($"/api/categories/{categoryId}", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var dbContext = _fixture.CreateDbContext();
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        category.Should().NotBeNull();
        category.Name.Should().Be(categoryName);
    }
}
