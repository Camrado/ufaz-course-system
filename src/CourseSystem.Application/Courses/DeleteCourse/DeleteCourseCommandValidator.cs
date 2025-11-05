using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Courses;
using FluentValidation;

namespace CourseSystem.Application.Courses.DeleteCourse;

internal sealed class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
{
    private readonly ICourseRepository _courseRepository;

    public DeleteCourseCommandValidator(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;

        RuleFor(x => x.CourseId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Course ID must not be empty.")
            .MustAsync(DoesCourseExist);
    }

    private async Task<bool> DoesCourseExist(int courseId, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(courseId, cancellationToken);

        if (course is null)
        {
            throw new NotFoundException("Course with ID '{0}' not found.", courseId);
        }

        return true;
    }
}
