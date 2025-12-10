using CourseSystem.Application.Categories.DeleteCategory;
using CourseSystem.Persistence.Categories;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Categories.Handlers;

public class DeleteCategoryHandlerTests
{
    private readonly DeleteCategoryCommandHandler _handler;
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _handler = new DeleteCategoryCommandHandler(_categoryRepository);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_Should_Pass(int categoryId)
    {
        var command = new DeleteCategoryCommand(categoryId);
        var category = Category.Create("Test Category");
        _categoryRepository.GetByIdAsync(command.CategoryId, CancellationToken.None).Returns(category);

        var result = await _handler.Handle(command, CancellationToken.None);

        await _categoryRepository.Received(1).GetByIdAsync(command.CategoryId, CancellationToken.None);
        _categoryRepository.Received(1).Delete(category, CancellationToken.None);
        result.Should().BeOfType<DeleteCategoryCommandResponse>();
        result.Id.Should().Be(result.Id);
    }
}
