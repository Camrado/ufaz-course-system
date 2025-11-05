using MediatR;

namespace CourseSystem.Application.Courses.CreateCourse;

public sealed record CreateCourseCommand(
    string Name,
    string? ShortDescription,
    string? Description,
    int CategoryId,
    int LanguageId,
    int? QuestionAnswerCount,
    bool IsActive,
    string? Slug
    ) : IRequest<CreateCourseCommandResponse>;
