using CourseSystem.Application.Categories.CreateCategory;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Categories;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Categories.Validators;

public class CreateCategoryValidatorTests
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly CreateCategoryCommandValidator _validator;

    public CreateCategoryValidatorTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _validator = new CreateCategoryCommandValidator(_categoryRepository);
    }

    [Theory]
    [InlineData("Valid Category")]
    public async Task Should_Pass_When_ValidData(string categoryName)
    {
        _categoryRepository.GetByNameAsync(categoryName, CancellationToken.None)
            .Returns((Category)null!);
        var command = new CreateCategoryCommand(categoryName);

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
        await _categoryRepository.Received(1).GetByNameAsync(categoryName, CancellationToken.None);
    }

    [Fact]
    public async Task Should_Fail_When_NameIsEmpty()
    {
        var command = new CreateCategoryCommand(string.Empty);

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Name is required.");
    }

    [Theory]
    [InlineData("Existing Category")]
    public async Task Should_Fail_When_NameAlreadyExists(string categoryName)
    {
        var existingCategory = Category.Create(categoryName);
        _categoryRepository.GetByNameAsync(categoryName, CancellationToken.None)
            .Returns(existingCategory);
        var command = new CreateCategoryCommand(categoryName);

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<ConflictException>();
        await _categoryRepository.Received(1).GetByNameAsync(categoryName, CancellationToken.None);
    }
}
