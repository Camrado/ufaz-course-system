using MediatR;

namespace CourseSystem.Application.Courses.UpdateCourse;

public sealed record UpdateCourseCommand : IRequest<UpdateCourseCommandResponse>
{
    public int CourseId { get; set; }
    public string Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int LanguageId { get; set; }
    public int? QuestionAnswerCount { get; set; }
    public bool IsActive { get; set; }
    public string? Slug { get; set; }
}
