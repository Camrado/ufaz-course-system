using CourseSystem.Application.Courses.CreateCourse;
using CourseSystem.Persistence.Courses;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Courses.Handlers;

public class CreateCourseHandlerTests
{
    private readonly ICourseRepository _courseRepository;
    private readonly CreateCourseCommandHandler _handler;

    public CreateCourseHandlerTests()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
        _handler = new CreateCourseCommandHandler(_courseRepository);
    }

    [Fact]
    public async Task Handle_Should_Pass()
    {
        var command = new CreateCourseCommand(
            "Test Course",
            "Short description",
            "Detailed description",
            1,
            1,
            10,
            true,
            "test-course"
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        _courseRepository.Received(1).Add(Arg.Is<Course>(c =>
            c.Name == command.Name &&
            c.ShortDescription == command.ShortDescription &&
            c.Description == command.Description &&
            c.CategoryId == command.CategoryId &&
            c.LanguageId == command.LanguageId &&
            c.QuestionAnswerCount == command.QuestionAnswerCount &&
            c.IsActive == command.IsActive &&
            c.Slug == command.Slug), CancellationToken.None);

        result.Should().BeOfType<CreateCourseCommandResponse>();
        result.Id.Should().Be(0); // Adjust this to return the actual ID in future
    }
}
