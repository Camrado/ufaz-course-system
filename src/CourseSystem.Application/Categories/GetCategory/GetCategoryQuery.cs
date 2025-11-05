using MediatR;

namespace CourseSystem.Application.Categories.GetCategory;

public sealed record GetCategoryQuery(int CategoryId) : IRequest<GetCategoryQueryResponse>;
