using System.Net;
using System.Net.Http.Json;
using CourseSystem.Application.Courses.CreateCourse;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Infrastructure;
using CourseSystem.Integration.Tests.Common;
using CourseSystem.Integration.Tests.Common.Response;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CourseSystem.Integration.Tests.Courses;

public class CreateCourseCommandTests : IClassFixture<IntegrationTestFixture>
{
    private readonly HttpClient _client;
    private readonly IntegrationTestFixture _fixture;

    public CreateCourseCommandTests(IntegrationTestFixture fixture)
    {
        _client = fixture.Client;
        _fixture = fixture;
    }

    [Theory]
    [InlineData("Test Course",
        "Short description of the test course",
        "Detailed description of the test course",
        1,
        1,
        5,
        true,
        "test-course")]
    [InlineData("Test Course 2",
        "Short description of the test course",
        "Detailed description of the test course",
        1,
        1,
        5,
        true,
        null)]
    [InlineData("Test Course 3",
        null,
        null,
        1,
        1,
        null,
        true,
        null)]
    public async Task Should_Create_Course_When_ValidDataProvided(
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var request = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var response = await _client.PostAsJsonAsync("/api/courses", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadFromJsonAsync<CreateCourseCommandResponse>();
        content.Should().NotBeNull();

        await using var dbContext = _fixture.CreateDbContext();
        var createdCourse = await dbContext.Courses.FirstOrDefaultAsync(c =>
            c.Name == name && c.CategoryId == categoryId && c.LanguageId == languageId);
        createdCourse.Should().NotBeNull();
        createdCourse.ShortDescription.Should().Be(shortDescription);
        createdCourse.Description.Should().Be(description);
        createdCourse.Slug.Should().Be(slug);
    }

    [Theory]
    [InlineData("", "Short Description", "Detailed Description", 1, 1, -2, true, "slug")]
    public async Task Should_Return_BadRequest_When_NameIsEmpty(
        string name,
        string shortDescription,
        string description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string slug)
    {
        var request = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var response = await _client.PostAsJsonAsync("/api/courses", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ValidationException));
        content.Detail.Should().Be("One or more validation errors occurred.");
        content.Status.Should().Be((int)HttpStatusCode.BadRequest);
        content.Errors.Should().ContainKey("Name");
        content.Errors["Name"].Should().Contain("Course name is required.");
    }

    [Theory]
    [InlineData("Test Course 13",
        "Veryyyyyyyyyyyyyy loooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooonggg descriptionnnn",
        "Detailed Description",
        1,
        1,
        5,
        true,
        null)]
    public async Task Should_Return_BadRequest_When_ShortDescriptionIsTooLong(
        string name,
        string shortDescription,
        string description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string slug)
    {
        var request = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);
        var response = await _client.PostAsJsonAsync("/api/courses", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ValidationException));
        content.Detail.Should().Be("One or more validation errors occurred.");
        content.Status.Should().Be((int)HttpStatusCode.BadRequest);
        content.Errors.Should().ContainKey("ShortDescription");
        content.Errors["ShortDescription"].Should().Contain("Short description must not exceed 200 characters.");
    }

    [Theory]
    [InlineData("Test Course 5", "Short Description", "Detailed Description", 1, 1, -3, true, "test-course-3")]
    public async Task Should_Return_BadRequest_When_QuestionAnswerCountIsNegative(
        string name,
        string shortDescription,
        string description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string slug)
    {
        var request = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var response = await _client.PostAsJsonAsync("/api/courses", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ValidationException));
        content.Detail.Should().Be("One or more validation errors occurred.");
        content.Status.Should().Be((int)HttpStatusCode.BadRequest);
        content.Errors.Should().ContainKey("QuestionAnswerCount");
        content.Errors["QuestionAnswerCount"].Should()
            .Contain("QuestionAnswerCount must be greater than or equal to 0.");
    }

    [Theory]
    [InlineData("Another Math Course",
        "It uses the same slug as course with id 1",
        "Detailed Description",
        9999,
        1,
        5,
        true,
        "introduction-to-mathematics")]
    public async Task Should_Return_ConflictException_When_SlugAlreadyExists(
        string name,
        string shortDescription,
        string description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string slug)
    {
        var request = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);
        var response = await _client.PostAsJsonAsync("/api/courses", request);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ConflictException));
        content.Detail.Should().Be($"Course with slug '{slug}' already exists.");
        content.Status.Should().Be((int)HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData("Test Course 6",
        "Short Description",
        "Detailed Description",
        CourseConstants.NonExistentCategoryId,
        1,
        5,
        true,
        "test-course-6")]
    public async Task Should_Return_NotFoundException_When_CategoryDoesNotExist(
        string name,
        string shortDescription,
        string description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string slug)
    {
        var request = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);
        var response = await _client.PostAsJsonAsync("/api/courses", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(NotFoundException));
        content.Detail.Should().Be($"Category with ID '{categoryId}' does not exist.");
        content.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("Test Course 7",
        "Short Description",
        "Detailed Description",
        1,
        CourseConstants.NonExistentLanguageId,
        5,
        true,
        "test-course-7")]
    public async Task Should_Return_NotFoundException_When_LanguageDoesNotExist(
        string name,
        string shortDescription,
        string description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string slug)
    {
        var request = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);
        var response = await _client.PostAsJsonAsync("/api/courses", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(NotFoundException));
        content.Detail.Should().Be($"Language with ID '{languageId}' does not exist.");
        content.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(CourseConstants.SampleCourseName,
        "Short Description",
        "Detailed Description",
        CourseConstants.SampleCourseCategoryId,
        CourseConstants.SampleCourseLanguageId,
        5,
        true,
        null)]
    public async Task Should_Return_ConflictException_When_CourseWithSameNameCategoryAndLanguageExists(
        string name,
        string shortDescription,
        string description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var request = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var response = await _client.PostAsJsonAsync("/api/courses", request);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ConflictException));
        content.Detail.Should().Be($"Course with name '{name}' already exists in the same category and language.");
        content.Status.Should().Be((int)HttpStatusCode.Conflict);
    }
}
