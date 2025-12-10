using CourseSystem.Application.Courses.CreateCourse;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Categories;
using CourseSystem.Persistence.Courses;
using CourseSystem.Persistence.Languages;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Courses.Validators;

public class CreateCourseValidatorTests
{
    private readonly ICourseRepository _courseRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly CreateCourseCommandValidator _validator;

    public CreateCourseValidatorTests()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
        _languageRepository = Substitute.For<ILanguageRepository>();
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _validator = new CreateCourseCommandValidator(_courseRepository, _languageRepository, _categoryRepository);
    }

    [Theory]
    [InlineData("Course 1", "Short description", "Description", 1, 1, 5, true, "course-1")]
    [InlineData("Course 2", "", "Description", 1, 1, 5, true, "course-1")]
    [InlineData("Course 3", null, null, 2, 2, null, false, null)]
    public async Task Should_Pass_When_ValidData(
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository.ExistsByNameCategoryAndLanguageAsync(name, categoryId, languageId, CancellationToken.None)
            .Returns(false);

        var command = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
        await _languageRepository.Received(1).GetByIdAsync(languageId, CancellationToken.None);
        await _courseRepository.Received(1).ExistsByNameCategoryAndLanguageAsync(
            name, categoryId, languageId, CancellationToken.None);
    }

    [Theory]
    [InlineData("", "Short description", "Description", 1, 1, 5, true, "course-1")]
    public async Task Should_Fail_When_NameIsEmpty(
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository.ExistsByNameCategoryAndLanguageAsync(name, categoryId, languageId, CancellationToken.None)
            .Returns(false);
        var command = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().Contain(e => e.ErrorMessage == "Course name is required.");
        result.IsValid.Should().BeFalse();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
        await _languageRepository.Received(1).GetByIdAsync(languageId, CancellationToken.None);
        await _courseRepository.Received(1).ExistsByNameCategoryAndLanguageAsync(
            name, categoryId, languageId, CancellationToken.None);
    }

    [Theory]
    [InlineData("Course 1",
        "Veryyyyyyyyyyyyyy loooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooonggg descriptionnnn",
        "Description",
        1,
        1,
        5,
        true,
        "course-1")]
    public async Task Should_Fail_When_ShortDescriptionIsTooLong(
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository.ExistsByNameCategoryAndLanguageAsync(name, categoryId, languageId, CancellationToken.None)
            .Returns(false);
        var command = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().Contain(e => e.ErrorMessage == "Short description must not exceed 200 characters.");
        result.IsValid.Should().BeFalse();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
        await _languageRepository.Received(1).GetByIdAsync(languageId, CancellationToken.None);
        await _courseRepository.Received(1).ExistsByNameCategoryAndLanguageAsync(
            name, categoryId, languageId, CancellationToken.None);
    }

    [Theory]
    [InlineData("Course 1", "Short description", "Description", 1, 1, -5, true, "course-1")]
    [InlineData("Course 1", "Short description", "Description", 1, 1, -1, true, "course-1")]
    public async Task Should_Fail_When_QuestionAnswerCountIsNegative(
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository.ExistsByNameCategoryAndLanguageAsync(name, categoryId, languageId, CancellationToken.None)
            .Returns(false);
        var command = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should()
            .Contain(e => e.ErrorMessage == "QuestionAnswerCount must be greater than or equal to 0.");
        result.IsValid.Should().BeFalse();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
        await _languageRepository.Received(1).GetByIdAsync(languageId, CancellationToken.None);
        await _courseRepository.Received(1).ExistsByNameCategoryAndLanguageAsync(
            name, categoryId, languageId, CancellationToken.None);
    }

    [Theory]
    [InlineData("Course 1", "Short description", "Description", 2, 1, 5, true, "course-1")]
    public async Task Should_Fail_When_CategoryDoesNotExist(
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns((Category?)null);
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository.ExistsByNameCategoryAndLanguageAsync(name, categoryId, languageId, CancellationToken.None)
            .Returns(false);
        var command = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Category with ID '{0}' does not exist.");
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
    }

    [Theory]
    [InlineData("Course 1", "Short description", "Description", 1, 2, 5, true, "course-1")]
    public async Task Should_Fail_When_LanguageDoesNotExist(
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns((Language?)null);
        _courseRepository.ExistsByNameCategoryAndLanguageAsync(name, categoryId, languageId, CancellationToken.None)
            .Returns(false);
        var command = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Language with ID '{0}' does not exist.");
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
    }

    [Theory]
    [InlineData("Course 1", "Short description", "Description", 1, 1, 5, true, "course-1")]
    public async Task Should_Fail_When_CourseWithSameNameCategoryAndLanguageExists(
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository.ExistsByNameCategoryAndLanguageAsync(name, categoryId, languageId, CancellationToken.None)
            .Returns(true);
        var command = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("Course with name '{0}' already exists in the same category and language.");
        await _courseRepository.Received(1).ExistsByNameCategoryAndLanguageAsync(
            name, categoryId, languageId, CancellationToken.None);
    }

    [Theory]
    [InlineData("Course 1", "Short description", "Description", 1, 1, 5, true, "course-1")]
    public async Task Should_Fail_When_SlugAlreadyExists(
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        var courseWithTheSameSlug = Course.Create(
            "Test Course",
            "Short Description",
            "Long Description",
            1,
            1,
            1,
            true,
            slug);
        _courseRepository.GetBySlugAsync(slug, CancellationToken.None).Returns(courseWithTheSameSlug);
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository.ExistsByNameCategoryAndLanguageAsync(name, categoryId, languageId, CancellationToken.None)
            .Returns(false);

        var command = new CreateCourseCommand(name,
            shortDescription,
            description,
            categoryId,
            languageId,
            questionAnswerCount,
            isActive,
            slug);

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("Course with slug '{0}' already exists.");
    }
}
