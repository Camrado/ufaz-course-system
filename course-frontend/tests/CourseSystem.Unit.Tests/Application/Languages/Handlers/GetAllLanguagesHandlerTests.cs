using CourseSystem.Application.Languages.GetAllLanguages;
using CourseSystem.Application.Languages.GetLanguage;
using CourseSystem.Persistence.Languages;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Languages.Handlers;

public class GetAllLanguagesHandlerTests
{
    private readonly GetAllLanguagesQueryHandler _handler;
    private readonly ILanguageRepository _languageRepository;

    public GetAllLanguagesHandlerTests()
    {
        _languageRepository = Substitute.For<ILanguageRepository>();

        _handler = new GetAllLanguagesQueryHandler(_languageRepository);
    }

    [Fact]
    public async Task Handle_Should_Pass()
    {
        var query = new GetAllLanguagesQuery();
        var languages = new List<Language>
        {
            Language.Create("EN"),
            Language.Create("AZ"),
            Language.Create("RU")
        };
        _languageRepository.GetAllAsync(CancellationToken.None).Returns(languages);

        var result = await _handler.Handle(query, CancellationToken.None);

        await _languageRepository.Received(1).GetAllAsync(CancellationToken.None);
        result.Should().BeOfType<GetAllLanguagesQueryResponse>();
        result.Languages.Should().Contain(languages
            .Select(c => new GetLanguageQueryResponse(
                c.Id,
                c.Code,
                c.CreatedAt,
                c.UpdatedAt)));
    }
}
