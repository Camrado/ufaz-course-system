using CourseSystem.Persistence.Languages;
using MediatR;

namespace CourseSystem.Application.Languages.DeleteLanguage;

internal sealed class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, DeleteLanguageCommandResponse>
{
    private readonly ILanguageRepository _languageRepository;

    public DeleteLanguageCommandHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<DeleteLanguageCommandResponse> Handle(DeleteLanguageCommand request,
        CancellationToken cancellationToken)
    {
        var language = await _languageRepository.GetByIdAsync(request.LanguageId, cancellationToken);

        _languageRepository.Delete(language!, cancellationToken);

        return new DeleteLanguageCommandResponse(language!.Id);
    }
}
