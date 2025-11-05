namespace CourseSystem.Application.Abstractions.Localization;

public interface ILanguageCodeProvider
{
    IReadOnlySet<string> GetValidLanguageCodes();
}
