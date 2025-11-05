using MediatR;

namespace CourseSystem.Application.Languages.DeleteLanguage;

public sealed record DeleteLanguageCommand(int LanguageId) : IRequest<DeleteLanguageCommandResponse>;
