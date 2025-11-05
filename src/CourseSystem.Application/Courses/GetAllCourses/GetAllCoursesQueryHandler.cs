using CourseSystem.Application.Courses.GetCourse;
using CourseSystem.Persistence.Courses;
using MediatR;

namespace CourseSystem.Application.Courses.GetAllCourses;

internal sealed class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, GetAllCoursesQueryResponse>
{
    private readonly ICourseRepository _courseRepository;

    public GetAllCoursesQueryHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<GetAllCoursesQueryResponse> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetAllAsync(cancellationToken);

        return new GetAllCoursesQueryResponse(
            courses.Select(c => new GetCourseQueryResponse(
                c.Id,
                c.Name,
                c.ShortDescription,
                c.Description,
                c.CategoryId,
                c.Category?.Name,
                c.LanguageId,
                c.Language?.Code,
                c.QuestionAnswerCount,
                c.IsActive,
                c.Slug,
                c.CreatedAt,
                c.UpdatedAt
            )).ToList());
    }
}
