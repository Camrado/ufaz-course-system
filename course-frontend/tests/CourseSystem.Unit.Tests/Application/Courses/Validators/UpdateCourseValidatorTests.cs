using CourseSystem.Application.Courses.UpdateCourse;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Categories;
using CourseSystem.Persistence.Courses;
using CourseSystem.Persistence.Languages;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Courses.Validators;

public class UpdateCourseValidatorTests
{
    private readonly ICourseRepository _courseRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly UpdateCourseCommandValidator _validator;
    private readonly Course _oldCourse = Course.Create(
        "Old Course",
        "Old Short Description",
        "Old Long Description",
        1,
        1,
        1,
        true,
        "old-course");

    public UpdateCourseValidatorTests()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
        _languageRepository = Substitute.For<ILanguageRepository>();
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _validator = new UpdateCourseCommandValidator(_courseRepository, _languageRepository, _categoryRepository);
    }

    [Theory]
    [InlineData(1, "Course 1", "Short description", "Description", 1, 1, 5, true, "course-1")]
    [InlineData(2, "Course 2", "", "Description", 1, 1, 5, true, "course-2")]
    [InlineData(3, "Course 3", null, null, 2, 2, null, false, null)]
    public async Task Should_Pass_When_ValidData(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _courseRepository.GetByIdAsync(courseId, CancellationToken.None)
            .Returns(_oldCourse);
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository
            .ExistsByNameCategoryAndLanguageExcludingIdAsync(courseId,
                name,
                categoryId,
                languageId,
                CancellationToken.None)
            .Returns(false);

        var command = new UpdateCourseCommand {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
        await _languageRepository.Received(1).GetByIdAsync(languageId, CancellationToken.None);
        await _courseRepository.Received(1).GetByIdAsync(courseId, CancellationToken.None);
        await _courseRepository.Received(1).ExistsByNameCategoryAndLanguageExcludingIdAsync(
            courseId,
            name,
            categoryId,
            languageId,
            CancellationToken.None);
    }

    [Fact]
    public async Task Should_Fail_When_CourseIdIsEmpty()
    {
        _categoryRepository.GetByIdAsync(1, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(1, CancellationToken.None)
            .Returns(Language.Create("en"));
        var command = new UpdateCourseCommand
        {
            CourseId = 0,
            Name = "Course 1",
            ShortDescription = "Short description",
            Description = "Description",
            CategoryId = 1,
            LanguageId = 1,
            QuestionAnswerCount = 5,
            IsActive = true,
            Slug = "course-1"
        };

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(f => f.ErrorMessage == "Course ID must not be empty.");
    }

    [Theory]
    [InlineData(1)]
    public async Task Should_Fail_When_CourseDoesNotExist(int courseId)
    {
        _courseRepository.GetByIdAsync(courseId, CancellationToken.None)
            .Returns((Course)null!);
        _categoryRepository.GetByIdAsync(1, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(1, CancellationToken.None)
            .Returns(Language.Create("en"));
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = "Course 1",
            ShortDescription = "Short description",
            Description = "Description",
            CategoryId = 1,
            LanguageId = 1,
            QuestionAnswerCount = 5,
            IsActive = true,
            Slug = "course-1"
        };

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Course with ID '{0}' not found.");
        await _courseRepository.Received(1).GetByIdAsync(courseId, CancellationToken.None);
    }

    [Theory]
    [InlineData(1, "", "Short description", "Description", 1, 1, 5, true, "course-1")]
    public async Task Should_Fail_When_NameIsEmpty(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _courseRepository.GetByIdAsync(courseId, CancellationToken.None)
            .Returns(_oldCourse);
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository
            .ExistsByNameCategoryAndLanguageExcludingIdAsync(courseId,
                name,
                categoryId,
                languageId,
                CancellationToken.None)
            .Returns(false);
        var command = new UpdateCourseCommand {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().Contain(e => e.ErrorMessage == "Course name is required.");
        result.IsValid.Should().BeFalse();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
        await _languageRepository.Received(1).GetByIdAsync(languageId, CancellationToken.None);
        await _courseRepository.Received(1).GetByIdAsync(courseId, CancellationToken.None);
        await _courseRepository.Received(1).ExistsByNameCategoryAndLanguageExcludingIdAsync(
            courseId,
            name,
            categoryId,
            languageId,
            CancellationToken.None);
    }

    [Theory]
    [InlineData(2,
        "Course 1",
        "Veryyyyyyyyyyyyyy loooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooonggg descriptionnnn",
        "Description",
        1,
        1,
        5,
        true,
        "course-1")]
    public async Task Should_Fail_When_ShortDescriptionIsTooLong(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _courseRepository.GetByIdAsync(courseId, CancellationToken.None)
            .Returns(_oldCourse);
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository
            .ExistsByNameCategoryAndLanguageExcludingIdAsync(courseId,
                name,
                categoryId,
                languageId,
                CancellationToken.None)
            .Returns(false);
        var command = new UpdateCourseCommand {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().Contain(e => e.ErrorMessage == "Short description must not exceed 200 characters.");
        result.IsValid.Should().BeFalse();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
        await _languageRepository.Received(1).GetByIdAsync(languageId, CancellationToken.None);
        await _courseRepository.Received(1).GetByIdAsync(courseId, CancellationToken.None);
        await _courseRepository.Received(1).ExistsByNameCategoryAndLanguageExcludingIdAsync(
            courseId,
            name,
            categoryId,
            languageId,
            CancellationToken.None);
    }

    [Theory]
    [InlineData(1, "Course 1", "Short description", "Description", 1, 1, -5, true, "course-1")]
    [InlineData(2, "Course 1", "Short description", "Description", 1, 1, -1, true, "course-1")]
    public async Task Should_Fail_When_QuestionAnswerCountIsNegative(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _courseRepository.GetByIdAsync(courseId, CancellationToken.None)
            .Returns(_oldCourse);
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository
            .ExistsByNameCategoryAndLanguageExcludingIdAsync(courseId,
                name,
                categoryId,
                languageId,
                CancellationToken.None)
            .Returns(false);
        var command = new UpdateCourseCommand {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should()
            .Contain(e => e.ErrorMessage == "QuestionAnswerCount must be greater than or equal to 0.");
        result.IsValid.Should().BeFalse();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
        await _languageRepository.Received(1).GetByIdAsync(languageId, CancellationToken.None);
        await _courseRepository.Received(1).GetByIdAsync(courseId, CancellationToken.None);
        await _courseRepository.Received(1).ExistsByNameCategoryAndLanguageExcludingIdAsync(
            courseId,
            name,
            categoryId,
            languageId,
            CancellationToken.None);
    }

    [Theory]
    [InlineData(1, "Course 1", "Short description", "Description", 2, 1, 5, true, "course-1")]
    public async Task Should_Fail_When_CategoryDoesNotExist(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _courseRepository.GetByIdAsync(courseId, CancellationToken.None)
            .Returns(_oldCourse);
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns((Category?)null);
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository
            .ExistsByNameCategoryAndLanguageExcludingIdAsync(courseId,
                name,
                categoryId,
                languageId,
                CancellationToken.None)
            .Returns(false);
        var command = new UpdateCourseCommand {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Category with ID '{0}' does not exist.");
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
    }

    [Theory]
    [InlineData(1, "Course 1", "Short description", "Description", 1, 2, 5, true, "course-1")]
    public async Task Should_Fail_When_LanguageDoesNotExist(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _courseRepository.GetByIdAsync(courseId, CancellationToken.None)
            .Returns(_oldCourse);
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns((Language?)null);
        _courseRepository
            .ExistsByNameCategoryAndLanguageExcludingIdAsync(courseId,
                name,
                categoryId,
                languageId,
                CancellationToken.None)
            .Returns(false);
        var command = new UpdateCourseCommand {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Language with ID '{0}' does not exist.");
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
    }

    [Theory]
    [InlineData(1, "Course 1", "Short description", "Description", 1, 1, 5, true, "course-1")]
    public async Task Should_Fail_When_CourseWithSameNameCategoryAndLanguageExists(
        int courseId,
        string name,
        string? shortDescription,
        string? description,
        int categoryId,
        int languageId,
        int? questionAnswerCount,
        bool isActive,
        string? slug)
    {
        _courseRepository.GetByIdAsync(courseId, CancellationToken.None)
            .Returns(_oldCourse);
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository
            .ExistsByNameCategoryAndLanguageExcludingIdAsync(courseId,
                name,
                categoryId,
                languageId,
                CancellationToken.None)
            .Returns(true);
        var command = new UpdateCourseCommand {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("Course with name '{0}' already exists in the same category and language.");
        await _courseRepository.Received(1).ExistsByNameCategoryAndLanguageExcludingIdAsync(
            courseId,
            name,
            categoryId,
            languageId,
            CancellationToken.None);
    }

    [Theory]
    [InlineData(1, "Course 1", "Short description", "Description", 1, 1, 5, true, "course-1")]
    public async Task Should_Fail_When_SlugAlreadyExists(
        int courseId,
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
        _courseRepository.GetByIdAsync(courseId, CancellationToken.None)
            .Returns(_oldCourse);
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None)
            .Returns(Category.Create("Category Name"));
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("en"));
        _courseRepository
            .ExistsByNameCategoryAndLanguageExcludingIdAsync(courseId,
                name,
                categoryId,
                languageId,
                CancellationToken.None)
            .Returns(false);

        var command = new UpdateCourseCommand {
            CourseId = courseId,
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            CategoryId = categoryId,
            LanguageId = languageId,
            QuestionAnswerCount = questionAnswerCount,
            IsActive = isActive,
            Slug = slug
        };

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("Course with slug '{0}' already exists.");
    }
}
