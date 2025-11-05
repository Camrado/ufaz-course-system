using CourseSystem.Persistence.Languages;
using MediatR;

namespace CourseSystem.Application.Languages.UpdateLanguage;

internal sealed class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, UpdateLanguageCommandResponse>
{
    private readonly ILanguageRepository _languageRepository;

    public UpdateLanguageCommandHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<UpdateLanguageCommandResponse> Handle(UpdateLanguageCommand request,
        CancellationToken cancellationToken)
    {
        var language = await _languageRepository.GetByIdAsync(request.LanguageId, cancellationToken);

        language!.UpdateLanguage(request.Code);

        return new UpdateLanguageCommandResponse(language.Id);
    }
}
