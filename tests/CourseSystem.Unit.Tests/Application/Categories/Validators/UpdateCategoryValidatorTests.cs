using CourseSystem.Application.Categories.UpdateCategory;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Categories;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Categories.Validators;

public class UpdateCategoryValidatorTests
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly UpdateCategoryCommandValidator _validator;

    public UpdateCategoryValidatorTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _validator = new UpdateCategoryCommandValidator(_categoryRepository);
    }

    [Theory]
    [InlineData(1, "New Test Category")]
    public async Task Should_Pass_When_ValidData(int categoryId, string categoryName)
    {
        var command = new UpdateCategoryCommand { CategoryId = categoryId, Name = categoryName};
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None).Returns(Category.Create("Old Test Category"));
        _categoryRepository.GetByNameAsync(categoryName, CancellationToken.None).Returns((Category)null!);

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
        await _categoryRepository.Received(1).GetByNameAsync(categoryName, CancellationToken.None);
    }

    [Fact]
    public async Task Should_Fail_When_CategoryIdIsEmpty()
    {
        var command = new UpdateCategoryCommand { CategoryId = 0 };

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(f => f.ErrorMessage == "Category ID must not be empty.");
    }

    [Theory]
    [InlineData(1, "New Test Category")]
    public async Task Should_Fail_When_CategoryDoesNotExist(int categoryId, string categoryName)
    {
        var command = new UpdateCategoryCommand { CategoryId = categoryId, Name = categoryName };
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None).Returns((Category)null!);

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<NotFoundException>();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
    }

    [Theory]
    [InlineData(1, "New Test Category")]
    public async Task Should_Fail_When_CategoryNameAlreadyExists(int categoryId, string categoryName)
    {
        var command = new UpdateCategoryCommand { CategoryId = categoryId, Name = categoryName };
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None).Returns(Category.Create("Old Test Category"));
        _categoryRepository.GetByNameAsync(categoryName, CancellationToken.None).Returns(Category.Create(categoryName));

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<ConflictException>();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
        await _categoryRepository.Received(1).GetByNameAsync(categoryName, CancellationToken.None);
    }
}
