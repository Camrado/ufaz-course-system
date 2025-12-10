using CourseSystem.Persistence.Languages;
using FluentAssertions;

namespace CourseSystem.Unit.Tests.Persistence.Languages;

public class LanguageTests
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        var code = "AZ";

        var category = Language.Create(code);

        category.Code.Should().Be(code);
    }
}
