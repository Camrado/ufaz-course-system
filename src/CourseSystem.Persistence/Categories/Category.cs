using CourseSystem.Persistence.Abstractions;
using CourseSystem.Persistence.Courses;

namespace CourseSystem.Persistence.Categories;

public class Category : BaseEntity
{
    public string Name { get; private set; }

    public virtual ICollection<Course> Courses { get; private set; } = new List<Course>();

    private Category(string name)
    {
        Name = name;
    }

    protected Category()
    {
    }

    public static Category Create(string name)
    {
        return new Category(name);
    }

    public void UpdateCategory(string name)
    {
        Name = name;
    }
}
