using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Categories;
using FluentValidation;

namespace CourseSystem.Application.Categories.GetCategory;

internal sealed class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryQueryValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(c => c.CategoryId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Category ID must not be empty.")
            .MustAsync(DoesCategoryExist);
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
}
