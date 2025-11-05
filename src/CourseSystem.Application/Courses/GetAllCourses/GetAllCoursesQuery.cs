using MediatR;

namespace CourseSystem.Application.Courses.GetAllCourses;

public sealed record GetAllCoursesQuery : IRequest<GetAllCoursesQueryResponse>;
