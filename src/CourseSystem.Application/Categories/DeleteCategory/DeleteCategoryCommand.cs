using MediatR;

namespace CourseSystem.Application.Categories.DeleteCategory;

public sealed record DeleteCategoryCommand(int CategoryId) : IRequest<DeleteCategoryCommandResponse>;
