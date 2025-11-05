using CourseSystem.Application.Abstractions.Localization;
using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Languages;
using FluentValidation;

namespace CourseSystem.Application.Languages.UpdateLanguage;

internal sealed class UpdateLanguageCommandValidator : AbstractValidator<UpdateLanguageCommand>
{
    private readonly ILanguageRepository _languageRepository;
    private readonly ILanguageCodeProvider _languageCodeProvider;

    public UpdateLanguageCommandValidator(ILanguageRepository languageRepository, ILanguageCodeProvider languageCodeProvider)
    {
        _languageRepository = languageRepository;
        _languageCodeProvider = languageCodeProvider;

        RuleFor(x => x.LanguageId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Language ID must not be empty.")
            .MustAsync(DoesLanguageExist);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required.")
            .Length(2)
            .WithMessage("Code must be exactly 2 characters long.")
            .Must(code => _languageCodeProvider.GetValidLanguageCodes().Contains(code.ToLower()))
            .WithMessage("Code must be a valid ISO 639-1 language code.")
            .MustAsync((command, code, cancellationToken) => IsUniqueCode(command.LanguageId, code, cancellationToken));
    }

    private async Task<bool> DoesLanguageExist(int languageId, CancellationToken cancellationToken)
    {
        var language = await _languageRepository.GetByIdAsync(languageId, cancellationToken);

        if (language is null)
        {
            throw new NotFoundException("Language with ID '{0}' not found.", languageId);
        }

        return true;
    }

    private async Task<bool> IsUniqueCode(int languageId, string code, CancellationToken cancellationToken)
    {
        var language = await _languageRepository.GetByCodeAsync(code, cancellationToken);

        if (language is not null && language.Id != languageId)
        {
            throw new ConflictException("Language with code '{0}' already exists.", code);
        }

        return true;
    }
}
