using System.Net;
using System.Net.Http.Json;
using CourseSystem.Application.Courses.UpdateCourse;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Infrastructure;
using CourseSystem.Integration.Tests.Common;
using CourseSystem.Integration.Tests.Common.Response;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CourseSystem.Integration.Tests.Courses;

public class UpdateCourseCommandTests : IClassFixture<IntegrationTestFixture>
{
    private readonly HttpClient _client;
    private readonly IntegrationTestFixture _fixture;

    public UpdateCourseCommandTests(IntegrationTestFixture fixture)
    {
        _client = fixture.Client;
        _fixture = fixture;
    }

    [Theory]
    [InlineData(CourseConstants.ThirdSampleCourseId,
        "Updated Course Name",
        "Updated Short Description",
        "Updated Description",
        2,
        2,
        5,
        true,
        "updated-course-name")]
    [InlineData(CourseConstants.SecondSampleCourseId, "Another Course Name", null, null, 1, 1, 0, false, null)]
    public async Task Should_Update_Course_When_ValidDataProvided(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<UpdateCourseCommandResponse>();
        content.Should().NotBeNull();
        content.Id.Should().Be(courseId);

        await using var dbContext = _fixture.CreateDbContext();
        var updatedCourse = await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        updatedCourse.Should().NotBeNull();
        updatedCourse.Name.Should().Be(name);
        updatedCourse.ShortDescription.Should().Be(shortDescription);
        updatedCourse.Description.Should().Be(description);
        updatedCourse.CategoryId.Should().Be(categoryId);
        updatedCourse.LanguageId.Should().Be(languageId);
        updatedCourse.QuestionAnswerCount.Should().Be(questionAnswerCount);
        updatedCourse.IsActive.Should().Be(isActive);
        updatedCourse.Slug.Should().Be(slug);
    }

    [Theory]
    [InlineData(CourseConstants.EmptyCourseId,
        "Updated Course Name 2",
        "Updated Short Description",
        "Updated Description",
        2,
        2,
        5,
        true,
        null)]
    public async Task Should_Return_BadRequest_When_CourseIdIsEmpty(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ValidationException));
        content.Detail.Should().Be("One or more validation errors occurred.");
        content.Status.Should().Be((int)HttpStatusCode.BadRequest);
        content.Errors.Should().ContainKey("CourseId");
        content.Errors["CourseId"].Should().Contain("Course ID must not be empty.");
    }

    [Theory]
    [InlineData(CourseConstants.NonExistentCourseId,
        "Updated Course Name 3",
        "Updated Short Description",
        "Updated Description",
        2,
        2,
        5,
        true,
        null)]
    public async Task Should_Return_NotFound_When_CourseDoesNotExist(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", command);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(NotFoundException));
        content.Detail.Should().Be($"Course with ID '{courseId}' not found.");
        content.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(CourseConstants.SampleCourseId,
        "",
        "Updated Short Description",
        "Updated Description",
        2,
        2,
        5,
        true,
        null)]
    public async Task Should_Return_BadRequest_When_NameIsEmpty(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", command);
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
    [InlineData(CourseConstants.SampleCourseId,
        "Updated Course Name 4",
        "Veryyyyyyyyyyyyyy loooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooonggg descriptionnnn",
        "Updated Description",
        2,
        2,
        5,
        true,
        null)]
    public async Task Should_Return_BadRequest_When_ShortDescriptionIsTooLong(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", command);
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
    [InlineData(CourseConstants.SampleCourseId,
        "Updated Course Name 5",
        "Updated Short Description",
        "Updated Detailed Description",
        2,
        1,
        -3,
        true,
        null)]
    public async Task Should_Return_BadRequest_When_QuestionAnswerCountIsNegative(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", command);
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
    [InlineData(CourseConstants.SecondSampleCourseId,
        "Updated Course Name 6",
        "Updated Short Description",
        "Updated Detailed Description",
        2,
        1,
        -3,
        true,
        CourseConstants.ExistingCourseSlug)]
    public async Task Should_Return_BadRequest_When_SlugAlreadyExists(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", command);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ConflictException));
        content.Detail.Should().Be($"Course with slug '{slug}' already exists.");
        content.Status.Should().Be((int)HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData(CourseConstants.SampleCourseId,
        "Updated Course Name 7",
        "Updated Short Description",
        "Updated Description",
        CourseConstants.NonExistentCategoryId,
        2,
        5,
        true,
        null)]
    public async Task Should_Return_BadRequest_When_CategoryDoesNotExist(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", command);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(NotFoundException));
        content.Detail.Should().Be($"Category with ID '{categoryId}' does not exist.");
        content.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(CourseConstants.SampleCourseId,
        "Updated Course Name 8",
        "Updated Short Description",
        "Updated Description",
        2,
        CourseConstants.NonExistentLanguageId,
        5,
        true,
        null)]
    public async Task Should_Return_BadRequest_When_LanguageDoesNotExist(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", command);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(NotFoundException));
        content.Detail.Should().Be($"Language with ID '{languageId}' does not exist.");
        content.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(CourseConstants.SecondSampleCourseId,
        CourseConstants.SampleCourseName,
        "Updated Short Description",
        "Updated Description",
        CourseConstants.SampleCourseCategoryId,
        CourseConstants.SampleCourseLanguageId,
        5,
        true,
        null)]
    public async Task Should_Return_ConflictException_When_CourseWithSameNameCategoryAndLanguageExists(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", command);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(ConflictException));
        content.Detail.Should().Be($"Course with name '{name}' already exists in the same category and language.");
        content.Status.Should().Be((int)HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData(CourseConstants.NotUpdatedSampleCourseId,
        CourseConstants.NotUpdatedSampleCourseName,
        CourseConstants.NotUpdatedSampleCourseShortDescription,
        CourseConstants.NotUpdatedSampleCourseDescription,
        CourseConstants.NotUpdatedSampleCourseCategoryId,
        CourseConstants.NotUpdatedSampleCourseLanguageId,
        CourseConstants.NotUpdatedSampleCourseQuestionAnswerCount,
        CourseConstants.NotUpdatedSampleCourseIsActive,
        CourseConstants.NotUpdatedSampleCourseSlug)]
    public async Task Should_Not_ThrowException_When_NoChangesMade(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<UpdateCourseCommandResponse>();
        content.Should().NotBeNull();
        content.Id.Should().Be(courseId);
    }
}
