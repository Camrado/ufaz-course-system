using MediatR;

namespace CourseSystem.Application.Languages.GetLanguage;

public sealed record GetLanguageQuery(int LanguageId) : IRequest<GetLanguageQueryResponse>;
