using CourseSystem.Persistence.Courses;
using MediatR;

namespace CourseSystem.Application.Courses.CreateCourse;

internal sealed class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CreateCourseCommandResponse>
{
    private readonly ICourseRepository _courseRepository;

    public CreateCourseCommandHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public Task<CreateCourseCommandResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = Course.Create(
            request.Name,
            request.ShortDescription,
            request.Description,
            request.CategoryId,
            request.LanguageId,
            request.QuestionAnswerCount,
            request.IsActive,
            request.Slug
        );

        _courseRepository.Add(course, cancellationToken);

        return Task.FromResult(new CreateCourseCommandResponse(course.Id));
    }
}
