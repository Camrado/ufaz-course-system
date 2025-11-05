using CourseSystem.Persistence.Categories;
using MediatR;

namespace CourseSystem.Application.Categories.GetCategory;

internal sealed class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, GetCategoryQueryResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryQueryHandler(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    public async Task<GetCategoryQueryResponse> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        return new GetCategoryQueryResponse(
            result!.Id,
            result.Name,
            result.CreatedAt,
            result.UpdatedAt);
    }
}
