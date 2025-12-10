using CourseSystem.Application.Categories.CreateCategory;
using CourseSystem.Persistence.Categories;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Categories.Handlers;

public class CreateCategoryHandlerTests
{
    private readonly CreateCategoryCommandHandler _handler;
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _handler = new CreateCategoryCommandHandler(_categoryRepository);
    }

    [Theory]
    [InlineData("Test Category")]
    public async Task Handle_Should_Pass(string categoryName)
    {
        var command = new CreateCategoryCommand(categoryName);

        var result = await _handler.Handle(command, CancellationToken.None);

        _categoryRepository.Received(1).Add(Arg.Is<Category>(c => c.Name == command.Name), CancellationToken.None);
        result.Should().BeOfType<CreateCategoryCommandResponse>();
        result.Id.Should().Be(0); // TODO: Adjust it in future to return the actual ID
    }
}
