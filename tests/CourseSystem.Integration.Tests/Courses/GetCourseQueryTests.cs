using System.Net;
using System.Net.Http.Json;
using CourseSystem.Application.Courses.GetCourse;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Infrastructure;
using CourseSystem.Integration.Tests.Common;
using CourseSystem.Integration.Tests.Common.Response;
using FluentAssertions;

namespace CourseSystem.Integration.Tests.Courses;

public class GetCourseQueryTests : IClassFixture<IntegrationTestFixture>
{
    private readonly HttpClient _client;

    public GetCourseQueryTests(IntegrationTestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Theory]
    [InlineData(CourseConstants.SampleCourseId)]
    public async Task Should_Return_Course_When_CourseExists(int courseId)
    {
        var response = await _client.GetAsync($"/api/courses/{courseId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<GetCourseQueryResponse>();

        content.Should().NotBeNull();
        content.Id.Should().Be(courseId);
        content.Name.Should().Be(CourseConstants.SampleCourseName);
        content.ShortDescription.Should().Be(CourseConstants.SampleCourseShortDescription);
        content.Description.Should().Be(CourseConstants.SampleCourseDescription);
        content.CategoryId.Should().Be(CourseConstants.SampleCourseCategoryId);
        content.CategoryName.Should().Be(CourseConstants.SampleCourseCategoryName);
        content.LanguageId.Should().Be(CourseConstants.SampleCourseLanguageId);
        content.LanguageCode.Should().Be(CourseConstants.SampleCourseLanguageCode);
        content.QuestionAnswerCount.Should().Be(CourseConstants.SampleCourseQuestionAnswerCount);
        content.IsActive.Should().Be(CourseConstants.SampleCourseIsActive);
        content.Slug.Should().Be(CourseConstants.SampleCourseSlug);
    }

    [Theory]
    [InlineData(CourseConstants.EmptyCourseId)]
    public async Task Should_Return_BadRequest_When_CourseIdIsEmpty(int courseId)
    {
        var response = await _client.GetAsync($"/api/courses/{courseId}");

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
    [InlineData(CourseConstants.NonExistentCourseId)]
    public async Task Should_Return_NotFound_When_CourseDoesNotExist(int courseId)
    {
        var response = await _client.GetAsync($"/api/courses/{courseId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var content = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        content.Should().NotBeNull();
        content.Type.Should().Be(nameof(NotFoundException));
        content.Detail.Should().Be($"Course with ID '{courseId}' not found.");
        content.Status.Should().Be((int)HttpStatusCode.NotFound);
    }
}
