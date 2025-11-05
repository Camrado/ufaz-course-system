using CourseSystem.Application.Abstractions.Localization;
using CourseSystem.Application.Languages.UpdateLanguage;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Languages;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Languages.Validators;

public class UpdateLanguageValidatorTests
{
    private readonly ILanguageRepository _languageRepository;
    private readonly ILanguageCodeProvider _languageCodeProvider;
    private readonly UpdateLanguageCommandValidator _validator;

    public UpdateLanguageValidatorTests()
    {
        _languageRepository = Substitute.For<ILanguageRepository>();
        _languageCodeProvider = Substitute.For<ILanguageCodeProvider>();
        _validator = new UpdateLanguageCommandValidator(_languageRepository, _languageCodeProvider);
    }

    [Theory]
    [InlineData(1, "EN")]
    [InlineData(2, "fr")]
    [InlineData(3, "aZ")]
    public async Task Should_Pass_When_ValidData(int languageId, string languageCode)
    {
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("ru"));
        _languageRepository.GetByCodeAsync(languageCode, CancellationToken.None)
            .Returns((Language)null!);
        _languageCodeProvider.GetValidLanguageCodes().Returns(new HashSet<string> { "en", "fr", "az" });
        var command = new UpdateLanguageCommand { LanguageId = languageId, Code = languageCode };

        var result = await _validator.ValidateAsync(command);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
        await _languageRepository.Received(1).GetByCodeAsync(languageCode, CancellationToken.None);
    }

    [Fact]
    public async Task Should_Fail_When_LanguageIdIsEmpty()
    {
        var command = new UpdateLanguageCommand { LanguageId = 0, Code = "ru" };

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(f => f.ErrorMessage == "Language ID must not be empty.");
    }

    [Theory]
    [InlineData(1, "ru")]
    public async Task Should_Fail_When_LanguageDoesNotExist(int languageId, string languageCode)
    {
        var command = new UpdateLanguageCommand { LanguageId = languageId, Code = languageCode };
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None).Returns((Language)null!);

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<NotFoundException>();
        await _languageRepository.Received(1).GetByIdAsync(languageId, CancellationToken.None);
    }

    [Theory]
    [InlineData(1)]
    public async Task Should_Fail_When_CodeIsEmpty(int languageId)
    {
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("ru"));
        var command = new UpdateLanguageCommand { LanguageId = languageId, Code = string.Empty };

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Code is required.");
    }

    [Theory]
    [InlineData(1, "EN")]
    [InlineData(2, "fr")]
    [InlineData(3, "aZ")]
    public async Task Should_Fail_When_CodeAlreadyExists(int languageId, string languageCode)
    {
        var existingLanguage = Language.Create(languageCode);
        _languageRepository.GetByCodeAsync(languageCode, CancellationToken.None)
            .Returns(existingLanguage);
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("ru"));
        var command = new UpdateLanguageCommand { LanguageId = languageId, Code = languageCode};

        var act = async () => await _validator.ValidateAsync(command);

        await act.Should().ThrowAsync<ConflictException>();
        await _languageRepository.Received(1).GetByCodeAsync(languageCode, CancellationToken.None);
    }

    [Theory]
    [InlineData(1, "Eng")]
    [InlineData(2, "french")]
    [InlineData(3, "aZe")]
    [InlineData(4, "a")]
    public async Task Should_Fail_When_LengthIsNotTwo(int languageId, string languageCode)
    {
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("ru"));
        var command = new UpdateLanguageCommand { LanguageId = languageId, Code = languageCode };

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Code must be exactly 2 characters long.");
    }

    [Theory]
    [InlineData(1, "ad")]
    [InlineData(2, "fe")]
    public async Task Should_Fail_When_CodeIsNotValid(int languageId, string languageCode)
    {
        _languageRepository.GetByIdAsync(languageId, CancellationToken.None)
            .Returns(Language.Create("ru"));
        _languageCodeProvider.GetValidLanguageCodes().Returns(new HashSet<string> { "en", "fr", "az" });
        var command = new UpdateLanguageCommand { LanguageId = languageId, Code = languageCode };

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Code must be a valid ISO 639-1 language code.");
    }
}
