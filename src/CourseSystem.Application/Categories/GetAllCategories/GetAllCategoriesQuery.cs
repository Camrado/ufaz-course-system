using MediatR;

namespace CourseSystem.Application.Categories.GetAllCategories;

public sealed record GetAllCategoriesQuery : IRequest<GetAllCategoriesQueryResponse>;
