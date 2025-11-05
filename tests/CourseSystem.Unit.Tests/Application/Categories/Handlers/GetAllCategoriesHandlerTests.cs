using CourseSystem.Application.Categories.GetAllCategories;
using CourseSystem.Application.Categories.GetCategory;
using CourseSystem.Persistence.Categories;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Categories.Handlers;

public class GetAllCategoriesHandlerTests
{
    private readonly GetAllCategoriesQueryHandler _handler;
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _handler = new GetAllCategoriesQueryHandler(_categoryRepository);
    }

    [Fact]
    public async Task Handle_Should_Pass()
    {
        var query = new GetAllCategoriesQuery();
        var categories = new List<Category>
        {
            Category.Create("Category 1"),
            Category.Create("Category 2")
        };
        _categoryRepository.GetAllAsync(CancellationToken.None).Returns(categories);

        var result = await _handler.Handle(query, CancellationToken.None);

        await _categoryRepository.Received(1).GetAllAsync(CancellationToken.None);
        result.Should().BeOfType<GetAllCategoriesQueryResponse>();
        result.Categories.Should().Contain(categories
            .Select(c => new GetCategoryQueryResponse(
                c.Id,
                c.Name,
                c.CreatedAt,
                c.UpdatedAt)));
    }
}
