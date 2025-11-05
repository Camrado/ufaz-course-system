using MediatR;

namespace CourseSystem.Application.Languages.GetAllLanguages;

public sealed record GetAllLanguagesQuery : IRequest<GetAllLanguagesQueryResponse>;
