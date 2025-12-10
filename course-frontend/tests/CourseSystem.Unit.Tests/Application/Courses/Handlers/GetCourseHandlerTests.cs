using CourseSystem.Application.Courses.GetCourse;
using CourseSystem.Persistence.Courses;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Courses.Handlers;

public class GetCourseHandlerTests
{
    private readonly GetCourseQueryHandler _handler;
    private readonly ICourseRepository _courseRepository;

    public GetCourseHandlerTests()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
        _handler = new GetCourseQueryHandler(_courseRepository);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_Should_Pass(int courseId)
    {
        var query = new GetCourseQuery(courseId);
        var course = Course.Create("Course 1",
            "Short description 1",
            "Detailed description 1",
            1,
            1,
            10,
            true,
            "course-1");
        _courseRepository.GetByIdAsync(query.CourseId, CancellationToken.None).Returns(course);

        var result = await _handler.Handle(query, CancellationToken.None);

        await _courseRepository.Received(1).GetByIdAsync(query.CourseId, CancellationToken.None);
        result.Should().BeOfType<GetCourseQueryResponse>();
        result.Id.Should().Be(course.Id);
        result.Name.Should().Be(course.Name);
        result.ShortDescription.Should().Be(course.ShortDescription);
        result.Description.Should().Be(course.Description);
        result.LanguageId.Should().Be(course.LanguageId);
        result.CategoryId.Should().Be(course.CategoryId);
        result.QuestionAnswerCount.Should().Be(course.QuestionAnswerCount);
        result.IsActive.Should().Be(course.IsActive);
        result.Slug.Should().Be(course.Slug);
        result.CreatedAt.Should().Be(course.CreatedAt);
        result.UpdatedAt.Should().Be(course.UpdatedAt);
    }
}
