using CourseSystem.Application.Categories.GetCategory;
using CourseSystem.Persistence.Categories;
using MediatR;

namespace CourseSystem.Application.Categories.GetAllCategories;

internal sealed class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, GetAllCategoriesQueryResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<GetAllCategoriesQueryResponse> Handle(GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        var responseCategories = categories
            .Select(c => new GetCategoryQueryResponse(
                c.Id,
                c.Name,
                c.CreatedAt,
                c.UpdatedAt))
            .ToList();

        return new GetAllCategoriesQueryResponse(responseCategories);
    }
}
