using CourseSystem.Application.Languages.GetLanguage;

namespace CourseSystem.Application.Languages.GetAllLanguages;

public record GetAllLanguagesQueryResponse(IReadOnlyList<GetLanguageQueryResponse> Languages);
