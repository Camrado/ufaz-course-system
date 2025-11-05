using CourseSystem.Persistence.Abstractions;
using CourseSystem.Persistence.Categories;
using CourseSystem.Persistence.Languages;

namespace CourseSystem.Persistence.Courses;

public class Course : BaseEntity
{
    public string Name { get; private set; }

    public string? ShortDescription { get; private set; }

    public string? Description { get; private set; }

    public int? CategoryId { get; private set; }

    public int? LanguageId { get; private set; }

    public int? QuestionAnswerCount { get; private set; }

    public bool IsActive { get; private set; }

    public string? Slug { get; private set; }

    public virtual Category? Category { get; private set; }

    public virtual Language? Language { get; private set; }

    protected Course()
    {
    }

    private Course(string name, string? shortDescription, string? description, int? categoryId, int? languageId,
        int? questionAnswerCount, bool isActive, string? slug)
    {
        Name = name;
        ShortDescription = shortDescription;
        Description = description;
        CategoryId = categoryId;
        LanguageId = languageId;
        QuestionAnswerCount = questionAnswerCount;
        IsActive = isActive;
        Slug = slug;
    }

    public static Course Create(string name, string? shortDescription, string? description, int? categoryId,
        int? languageId, int? questionAnswerCount, bool isActive, string? slug)
    {
        return new Course(name, shortDescription, description, categoryId, languageId, questionAnswerCount, isActive,
            slug);
    }

    public void UpdateCourse(string name, string? shortDescription, string? description, int? categoryId, int? languageId,
        int? questionAnswerCount, bool isActive, string? slug)
    {
        Name = name;
        ShortDescription = shortDescription;
        Description = description;
        CategoryId = categoryId;
        LanguageId = languageId;
        QuestionAnswerCount = questionAnswerCount;
        IsActive = isActive;
        Slug = slug;
    }
}
