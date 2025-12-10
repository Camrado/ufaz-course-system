using CourseSystem.Application.Courses.UpdateCourse;
using CourseSystem.Persistence.Courses;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Courses.Handlers;

public class UpdateCourseHandlerTests
{
    private readonly UpdateCourseCommandHandler _handler;
    private readonly ICourseRepository _courseRepository;

    public UpdateCourseHandlerTests()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
        _handler = new UpdateCourseCommandHandler(_courseRepository);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_Should_Pass(int courseId)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = "Updated Course",
            ShortDescription = "Updated Short Description",
            Description = "Updated Description",
            LanguageId = 1,
            CategoryId = 1,
            QuestionAnswerCount = 5,
            IsActive = true,
            Slug = "updated-course"
        };

        var oldCourse = Course.Create("Old Course",
            "Old Short Description",
            "Old Description",
            1,
            1,
            10,
            true,
            "old-course");
        _courseRepository.GetByIdAsync(command.CourseId, CancellationToken.None).Returns(oldCourse);

        var result = await _handler.Handle(command, CancellationToken.None);

        await _courseRepository.Received(1).GetByIdAsync(command.CourseId, CancellationToken.None);
        result.Should().BeOfType<UpdateCourseCommandResponse>();
    }
}
