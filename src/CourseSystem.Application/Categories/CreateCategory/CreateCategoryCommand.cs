using MediatR;

namespace CourseSystem.Application.Categories.CreateCategory;

public sealed record CreateCategoryCommand(string Name) : IRequest<CreateCategoryCommandResponse>;
