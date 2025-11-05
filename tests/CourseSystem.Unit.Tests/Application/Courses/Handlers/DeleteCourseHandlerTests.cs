using CourseSystem.Application.Courses.DeleteCourse;
using CourseSystem.Persistence.Courses;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Courses.Handlers;

public class DeleteCourseHandlerTests
{
    private readonly ICourseRepository _courseRepository;
    private readonly DeleteCourseCommandHandler _handler;

    public DeleteCourseHandlerTests()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
        _handler = new DeleteCourseCommandHandler(_courseRepository);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_Should_Pass(int courseId)
    {
        var command = new DeleteCourseCommand(courseId);
        var course = Course.Create("Test Course",
            "This is a short description",
            "This is a detailed description of the course.",
            1,
            1,
            10,
            true,
            "test-course");
        _courseRepository.GetByIdAsync(command.CourseId, CancellationToken.None).Returns(course);

        var result = await _handler.Handle(command, CancellationToken.None);

        await _courseRepository.Received(1).GetByIdAsync(command.CourseId, CancellationToken.None);
        _courseRepository.Received(1).Delete(course, CancellationToken.None);
        result.Should().BeOfType<DeleteCourseCommandResponse>();
    }
}
