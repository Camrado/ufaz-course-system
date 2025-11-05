using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Categories;
using CourseSystem.Persistence.Courses;
using CourseSystem.Persistence.Languages;
using FluentValidation;

namespace CourseSystem.Application.Courses.CreateCourse;

internal sealed class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ILanguageRepository _languageRepository;

    public CreateCourseCommandValidator(ICourseRepository courseRepository,
        ILanguageRepository languageRepository,
        ICategoryRepository categoryRepository)
    {
        _languageRepository = languageRepository;
        _categoryRepository = categoryRepository;
        _courseRepository = courseRepository;

        RuleFor(x => x)
            .MustAsync(IsUniqueNameCategoryLanguageGroup);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Course name is required.");

        RuleFor(x => x.ShortDescription)
            .MaximumLength(200)
            .WithMessage("Short description must not exceed 200 characters.");

        RuleFor(x => x.Slug)
            .MustAsync(IsSlugUnique!)
            .When(x => !string.IsNullOrEmpty(x.Slug));

        RuleFor(x => x.QuestionAnswerCount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("QuestionAnswerCount must be greater than or equal to 0.");

        RuleFor(x => x.CategoryId)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("CategoryId is required.")
            .MustAsync(DoesCategoryExist);

        RuleFor(x => x.LanguageId)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("LanguageId is required.")
            .MustAsync(DoesLanguageExist);
    }

    private async Task<bool> IsUniqueNameCategoryLanguageGroup(CreateCourseCommand command,
        CancellationToken cancellationToken)
    {
        var doesExistByNameAndCategoryIdAndLanguageId = await _courseRepository.ExistsByNameCategoryAndLanguageAsync(
            command.Name,
            command.CategoryId,
            command.LanguageId,
            cancellationToken);

        if (doesExistByNameAndCategoryIdAndLanguageId)
        {
            throw new ConflictException(
                "Course with name '{0}' already exists in the same category and language.", command.Name);
        }

        return true;
    }

    private async Task<bool> IsSlugUnique(string slug, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetBySlugAsync(slug, cancellationToken);

        if (course is not null)
        {
            throw new ConflictException("Course with slug '{0}' already exists.", slug);
        }

        return true;
    }

    private async Task<bool> DoesCategoryExist(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException("Category with ID '{0}' does not exist.", categoryId);
        }

        return true;
    }

    private async Task<bool> DoesLanguageExist(int languageId, CancellationToken cancellationToken)
    {
        var language = await _languageRepository.GetByIdAsync(languageId, cancellationToken);

        if (language is null)
        {
            throw new NotFoundException("Language with ID '{0}' does not exist.", languageId);
        }

        return true;
    }
}
