using CourseSystem.Persistence.Courses;
using MediatR;

namespace CourseSystem.Application.Courses.UpdateCourse;

internal sealed class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, UpdateCourseCommandResponse>
{
    private readonly ICourseRepository _courseRepository;

    public UpdateCourseCommandHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<UpdateCourseCommandResponse> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);

        course!.UpdateCourse(
            request.Name,
            request.ShortDescription,
            request.Description,
            request.CategoryId,
            request.LanguageId,
            request.QuestionAnswerCount,
            request.IsActive,
            request.Slug
        );

        return new UpdateCourseCommandResponse(course.Id);
    }
}
