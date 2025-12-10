using CourseSystem.Persistence.Courses;
using FluentAssertions;

namespace CourseSystem.Unit.Tests.Persistence.Courses;

public class CourseTests
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        var name = "Test Course";
        var shortDescription = "Short description";
        var description = "Detailed description of the course.";
        var categoryId = 1;
        var languageId = 1;
        var questionAnswerCount = 10;
        var isActive = true;
        var slug = "test-course";

        var course = Course.Create(
            name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug
        );

        course.Name.Should().Be(name);
        course.ShortDescription.Should().Be(shortDescription);
        course.Description.Should().Be(description);
        course.CategoryId.Should().Be(categoryId);
        course.LanguageId.Should().Be(languageId);
        course.QuestionAnswerCount.Should().Be(questionAnswerCount);
        course.IsActive.Should().Be(isActive);
        course.Slug.Should().Be(slug);
    }
}
