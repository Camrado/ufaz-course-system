using CourseSystem.Persistence.Categories;
using FluentAssertions;

namespace CourseSystem.Unit.Tests.Persistence.Categories;

public class CategoryTests
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        var name = "Test Category";

        var category = Category.Create(name);

        category.Name.Should().Be(name);
    }
}
