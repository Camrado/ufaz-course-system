using CourseSystem.Exceptions.Exceptions;
using CourseSystem.Persistence.Languages;
using FluentValidation;

namespace CourseSystem.Application.Languages.GetLanguage;

internal sealed class GetLanguageQueryValidator : AbstractValidator<GetLanguageQuery>
{
    private readonly ILanguageRepository _languageRepository;

    public GetLanguageQueryValidator(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;

        RuleFor(x => x.LanguageId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Language ID must not be empty.")
            .MustAsync(DoesLanguageExist);
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
}
