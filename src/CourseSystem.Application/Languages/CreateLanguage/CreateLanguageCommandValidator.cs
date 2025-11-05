using CourseSystem.Application.Abstractions.Localization;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Languages;
using FluentValidation;

namespace CourseSystem.Application.Languages.CreateLanguage;

internal sealed class CreateLanguageCommandValidator : AbstractValidator<CreateLanguageCommand>
{
    private readonly ILanguageRepository _languageRepository;
    private readonly ILanguageCodeProvider _languageCodeProvider;

    public CreateLanguageCommandValidator(ILanguageRepository languageRepository, ILanguageCodeProvider languageCodeProvider)
    {
        _languageRepository = languageRepository;
        _languageCodeProvider = languageCodeProvider;

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required.")
            .Length(2)
            .WithMessage("Code must be exactly 2 characters long.")
            .Must(code => _languageCodeProvider.GetValidLanguageCodes().Contains(code.ToLower()))
            .WithMessage("Code must be a valid ISO 639-1 language code.")
            .MustAsync(IsUniqueCode);
    }

    private async Task<bool> IsUniqueCode(string code, CancellationToken cancellationToken)
    {
        var language = await _languageRepository.GetByCodeAsync(code, cancellationToken);

        if (language is not null)
        {
            throw new ConflictException("Language with code '{0}' already exists.", code);
        }

        return true;
    }
}
