using MediatR;

namespace CourseSystem.Application.Courses.GetCourse;

public sealed record GetCourseQuery(int CourseId) : IRequest<GetCourseQueryResponse>;
