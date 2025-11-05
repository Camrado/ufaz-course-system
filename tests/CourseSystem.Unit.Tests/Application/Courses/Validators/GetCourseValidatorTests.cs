using CourseSystem.Application.Courses.GetCourse;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Courses;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Courses.Validators;

public class GetCourseValidatorTests
{
    private readonly ICourseRepository _courseRepository;
    private readonly GetCourseQueryValidator _validator;

    public GetCourseValidatorTests()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
        _validator = new GetCourseQueryValidator(_courseRepository);
    }

    [Theory]
    [InlineData(1)]
    public async Task Should_Pass_When_ValidData(int courseId)
    {
        var command = new GetCourseQuery(courseId);
        var course = Course.Create(
            "Test Course",
            "Short Description",
            "Long Description",
            1,
            1,
            1,
            true,
            "test-slug");
        _courseRepository.GetByIdAsync(courseId, CancellationToken.None).Returns(course);

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
        await _courseRepository.Received(1).GetByIdAsync(courseId, CancellationToken.None);
    }

    [Fact]
    public async Task Should_Fail_When_CourseIdIsEmpty()
    {
        var command = new GetCourseQuery(0);

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(f => f.ErrorMessage == "Course ID must not be empty.");
    }

    [Theory]
    [InlineData(1)]
    public async Task Should_Fail_When_CourseDoesNotExist(int courseId)
    {
        var command = new GetCourseQuery(courseId);
        _courseRepository.GetByIdAsync(courseId, CancellationToken.None).Returns((Course)null!);

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Course with ID '{0}' not found.");
        await _courseRepository.Received(1).GetByIdAsync(courseId, CancellationToken.None);
    }
}
