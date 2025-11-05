using CourseSystem.Persistence.Courses;
using MediatR;

namespace CourseSystem.Application.Courses.GetCourse;

internal sealed class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, GetCourseQueryResponse>
{
    private readonly ICourseRepository _courseRepository;

    public GetCourseQueryHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<GetCourseQueryResponse> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);

        return new GetCourseQueryResponse(
            course!.Id,
            course.Name,
            course.ShortDescription,
            course.Description,
            course.CategoryId,
            course.Category?.Name,
            course.LanguageId,
            course.Language?.Code,
            course.QuestionAnswerCount,
            course.IsActive,
            course.Slug,
            course.CreatedAt,
            course.UpdatedAt);
    }
}
