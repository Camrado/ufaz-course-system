using CourseSystem.Persistence.Categories;
using MediatR;

namespace CourseSystem.Application.Categories.CreateCategory;

internal sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var newCategory = Category.Create(request.Name);

        _categoryRepository.Add(newCategory, cancellationToken);

        return Task.FromResult(new CreateCategoryCommandResponse(0)); // TODO: fix this to return the created entity ID
    }
}
