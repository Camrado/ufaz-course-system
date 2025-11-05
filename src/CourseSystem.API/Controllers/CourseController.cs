using CourseSystem.Application.Courses.CreateCourse;
using CourseSystem.Application.Courses.DeleteCourse;
using CourseSystem.Application.Courses.GetAllCourses;
using CourseSystem.Application.Courses.GetCourse;
using CourseSystem.Application.Courses.UpdateCourse;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseSystem.API.Controllers;

[ApiController]
[Route("api/courses")]
public sealed class CourseController : ControllerBase
{
    private readonly IMediator _sender;

    public CourseController(IMediator sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetAllCourses(CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(new GetAllCoursesQuery(), cancellationToken));
    }

    [HttpGet("{courseId:int}")]
    public async Task<IActionResult> GetCourseById(int courseId, CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(new GetCourseQuery(courseId), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetCourseById), new { courseId = result.Id }, result);
    }

    [HttpPut("{courseId:int}")]
    public async Task<IActionResult> UpdateCourse(int courseId, [FromBody] UpdateCourseCommand request,
        CancellationToken cancellationToken)
    {
        request.CourseId = courseId;
        return Ok(await _sender.Send(request, cancellationToken));
    }

    [HttpDelete("{courseId:int}")]
    public async Task<IActionResult> DeleteCourse(int courseId, CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(new DeleteCourseCommand(courseId), cancellationToken));
    }
}
