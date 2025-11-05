using CourseSystem.Application.Courses.GetAllCourses;
using CourseSystem.Persistence.Courses;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Courses.Handlers;

public class GetAllCoursesHandlerTests
{
    private readonly ICourseRepository _courseRepository;
    private readonly GetAllCoursesQueryHandler _handler;

    public GetAllCoursesHandlerTests()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
        _handler = new GetAllCoursesQueryHandler(_courseRepository);
    }

    [Fact]
    public async Task Handle_Should_Pass()
    {
        var query = new GetAllCoursesQuery();
        var courses = new List<Course>
        {
            Course.Create("Course 1", "Short description 1", "Detailed description 1", 1, 1, 10, true, "course-1"),
            Course.Create("Course 2", "Short description 2", "Detailed description 2", 2, 2, 20, true, "course-2")
        };
        _courseRepository.GetAllAsync(CancellationToken.None).Returns(courses);

        var result = await _handler.Handle(query, CancellationToken.None);

        await _courseRepository.Received(1).GetAllAsync(CancellationToken.None);
        result.Should().BeOfType<GetAllCoursesQueryResponse>();
        result.Courses.Should().HaveCount(2);
    }
}
