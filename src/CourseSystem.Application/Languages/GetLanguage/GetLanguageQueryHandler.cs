using CourseSystem.Persistence.Languages;
using MediatR;

namespace CourseSystem.Application.Languages.GetLanguage;

internal sealed class GetLanguageQueryHandler : IRequestHandler<GetLanguageQuery, GetLanguageQueryResponse>
{
    private readonly ILanguageRepository _languageRepository;

    public GetLanguageQueryHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<GetLanguageQueryResponse> Handle(GetLanguageQuery request, CancellationToken cancellationToken)
    {
        var result = await _languageRepository.GetByIdAsync(request.LanguageId, cancellationToken);

        return new GetLanguageQueryResponse(
            result!.Id,
            result.Code,
            result.CreatedAt,
            result.UpdatedAt);
    }
}
