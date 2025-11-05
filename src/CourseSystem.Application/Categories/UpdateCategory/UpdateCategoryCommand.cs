using MediatR;

namespace CourseSystem.Application.Categories.UpdateCategory;

public sealed record UpdateCategoryCommand : IRequest<UpdateCategoryCommandResponse>
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
};
