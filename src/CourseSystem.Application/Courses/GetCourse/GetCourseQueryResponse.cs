namespace CourseSystem.Application.Courses.GetCourse;

public record GetCourseQueryResponse(
    int Id,
    string Name,
    string ShortDescription,
    string Description,
    int? CategoryId,
    string? CategoryName,
    int? LanguageId,
    string? LanguageCode,
    int? QuestionAnswerCount,
    bool IsActive,
    string? Slug,
    DateTime CreatedAt,
    DateTime UpdatedAt);
