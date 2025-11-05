using CourseSystem.Application.Categories.DeleteCategory;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Categories;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Categories.Validators;

public class DeleteCategoryValidatorTests
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly DeleteCategoryCommandValidator _validator;

    public DeleteCategoryValidatorTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _validator = new DeleteCategoryCommandValidator(_categoryRepository);
    }

    [Theory]
    [InlineData(1)]
    public async Task Should_Pass_When_ValidData(int categoryId)
    {
        var command = new DeleteCategoryCommand(categoryId);
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None).Returns(Category.Create("Test Category"));

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
    }

    [Fact]
    public async Task Should_Fail_When_CategoryIdIsEmpty()
    {
        var command = new DeleteCategoryCommand(0);

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(f => f.ErrorMessage == "Category ID must not be empty.");
    }

    [Theory]
    [InlineData(1)]
    public async Task Should_Fail_When_CategoryDoesNotExist(int categoryId)
    {
        var command = new DeleteCategoryCommand(categoryId);
        _categoryRepository.GetByIdAsync(categoryId, CancellationToken.None).Returns((Category)null!);

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<NotFoundException>();
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, CancellationToken.None);
    }
}
