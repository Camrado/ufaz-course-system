using CourseSystem.Application.Categories.GetCategory;
using CourseSystem.Persistence.Categories;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Categories.Handlers;

public class GetCategoryHandlerTests
{
    private readonly GetCategoryQueryHandler _handler;
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _handler = new GetCategoryQueryHandler(_categoryRepository);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_Should_Pass(int categoryId)
    {
        var query = new GetCategoryQuery(categoryId);
        var category = Category.Create("Test Category");
        _categoryRepository.GetByIdAsync(query.CategoryId, CancellationToken.None).Returns(category);

        var result = await _handler.Handle(query, CancellationToken.None);

        await _categoryRepository.Received(1).GetByIdAsync(query.CategoryId, CancellationToken.None);
        result.Should().BeOfType<GetCategoryQueryResponse>();
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be(category.Name);
        result.CreatedAt.Should().Be(category.CreatedAt);
        result.UpdatedAt.Should().Be(category.UpdatedAt);
    }
}
