using CourseSystem.Persistence.Languages;
using MediatR;

namespace CourseSystem.Application.Languages.CreateLanguage;

internal sealed class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, CreateLanguageCommandResponse>
{
    private readonly ILanguageRepository _languageRepository;

    public CreateLanguageCommandHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public Task<CreateLanguageCommandResponse> Handle(CreateLanguageCommand request,
        CancellationToken cancellationToken)
    {
        var language = Language.Create(request.Code);

        _languageRepository.Add(language, cancellationToken);

        return Task.FromResult(new CreateLanguageCommandResponse(0)); // TODO: fix this to return the created entity ID
    }
}
