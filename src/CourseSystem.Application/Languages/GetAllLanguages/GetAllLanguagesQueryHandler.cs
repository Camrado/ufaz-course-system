using CourseSystem.Application.Languages.GetLanguage;
using CourseSystem.Persistence.Languages;
using MediatR;

namespace CourseSystem.Application.Languages.GetAllLanguages;

internal sealed class GetAllLanguagesQueryHandler : IRequestHandler<GetAllLanguagesQuery, GetAllLanguagesQueryResponse>
{
    private readonly ILanguageRepository _languageRepository;

    public GetAllLanguagesQueryHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<GetAllLanguagesQueryResponse> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
    {
        var languages = await _languageRepository.GetAllAsync(cancellationToken);

        var languageResponses = languages
            .Select(language => new GetLanguageQueryResponse(
                language.Id,
                language.Code,
                language.CreatedAt,
                language.UpdatedAt))
            .ToList();

        return new GetAllLanguagesQueryResponse(languageResponses);
    }
}
