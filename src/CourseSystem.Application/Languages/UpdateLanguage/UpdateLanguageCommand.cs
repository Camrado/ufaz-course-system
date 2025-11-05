using MediatR;

namespace CourseSystem.Application.Languages.UpdateLanguage;

public sealed record UpdateLanguageCommand : IRequest<UpdateLanguageCommandResponse>
{
    public int LanguageId { get; set; }
    public string Code { get; set; }
}
