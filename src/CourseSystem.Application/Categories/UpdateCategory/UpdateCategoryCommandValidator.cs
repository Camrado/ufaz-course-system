using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Categories;
using FluentValidation;

namespace CourseSystem.Application.Categories.UpdateCategory;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.CategoryId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Category ID must not be empty.")
            .MustAsync(DoesCategoryExist);

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MustAsync((command, name, cancellationToken) => IsUniqueTitle(command.CategoryId, name, cancellationToken));
    }

    private async Task<bool> DoesCategoryExist(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException("Category with ID '{0}' not found.", categoryId);
        }

        return true;
    }

    private async Task<bool> IsUniqueTitle(int categoryId, string title, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByNameAsync(title, cancellationToken);

        if (category is not null && category.Id != categoryId)
        {
            throw new ConflictException("Category with title '{0}' already exists.", title);
        }

        return true;
    }
}
