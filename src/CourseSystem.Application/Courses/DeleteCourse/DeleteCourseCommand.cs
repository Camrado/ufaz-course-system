using MediatR;

namespace CourseSystem.Application.Courses.DeleteCourse;

public sealed record DeleteCourseCommand(int CourseId) : IRequest<DeleteCourseCommandResponse>;
