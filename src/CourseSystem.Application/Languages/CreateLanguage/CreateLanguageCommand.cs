using MediatR;

namespace CourseSystem.Application.Languages.CreateLanguage;

public sealed record CreateLanguageCommand(string Code) : IRequest<CreateLanguageCommandResponse>;
