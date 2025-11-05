using CourseSystem.Application.Categories.GetCategory;

namespace CourseSystem.Application.Categories.GetAllCategories;

public record GetAllCategoriesQueryResponse(IReadOnlyList<GetCategoryQueryResponse> Categories);
