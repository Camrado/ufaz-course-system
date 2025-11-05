using CourseSystem.Application.Courses.GetCourse;

namespace CourseSystem.Application.Courses.GetAllCourses;

public record GetAllCoursesQueryResponse(IReadOnlyList<GetCourseQueryResponse> Courses);
