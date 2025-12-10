using CourseSystem.Application.Abstractions.Localization;
using CourseSystem.Application.Languages.CreateLanguage;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Languages;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Languages.Validators;

public class CreateLanguageValidatorTests
{
    private readonly ILanguageRepository _languageRepository;
    private readonly ILanguageCodeProvider _languageCodeProvider;
    private readonly CreateLanguageCommandValidator _validator;

    public CreateLanguageValidatorTests()
    {
        _languageRepository = Substitute.For<ILanguageRepository>();
        _languageCodeProvider = Substitute.For<ILanguageCodeProvider>();
        _validator = new CreateLanguageCommandValidator(_languageRepository, _languageCodeProvider);
    }

    [Theory]
    [InlineData("EN")]
    [InlineData("fr")]
    [InlineData("aZ")]
    public async Task Should_Pass_When_ValidData(string languageCode)
    {
        _languageRepository.GetByCodeAsync(languageCode, CancellationToken.None)
            .Returns((Language)null!);
        _languageCodeProvider.GetValidLanguageCodes().Returns(new HashSet<string> { "en", "fr", "az" });
        var command = new CreateLanguageCommand(languageCode);

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
        await _languageRepository.Received(1).GetByCodeAsync(languageCode, CancellationToken.None);
    }

    [Fact]
    public async Task Should_Fail_When_CodeIsEmpty()
    {
        var command = new CreateLanguageCommand(string.Empty);

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Code is required.");
    }

    [Theory]
    [InlineData("EN")]
    [InlineData("fr")]
    [InlineData("aZ")]
    public async Task Should_Fail_When_CodeAlreadyExists(string languageCode)
    {
        var existingLanguage = Language.Create(languageCode);
        _languageRepository.GetByCodeAsync(languageCode, CancellationToken.None)
            .Returns(existingLanguage);
        var command = new CreateLanguageCommand(languageCode);

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<ConflictException>();
        await _languageRepository.Received(1).GetByCodeAsync(languageCode, CancellationToken.None);
    }

    [Theory]
    [InlineData("Eng")]
    [InlineData("french")]
    [InlineData("aZe")]
    [InlineData("a")]
    public async Task Should_Fail_When_LengthIsNotTwo(string languageCode)
    {
        var command = new CreateLanguageCommand(languageCode);

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Code must be exactly 2 characters long.");
    }

    [Theory]
    [InlineData("ad")]
    [InlineData("fe")]
    public async Task Should_Fail_When_CodeIsNotValid(string languageCode)
    {
        _languageCodeProvider.GetValidLanguageCodes().Returns(new HashSet<string> { "en", "fr", "az" });
        var command = new CreateLanguageCommand(languageCode);

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Code must be a valid ISO 639-1 language code.");
    }
}
