using CourseSystem.Application.Languages.GetLanguage;
using CourseSystem.Persistence.Languages;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Languages.Handlers;

public class GetLanguageHandlerTests
{
    private readonly GetLanguageQueryHandler _handler;
    private readonly ILanguageRepository _languageRepository;

    public GetLanguageHandlerTests()
    {
        _languageRepository = Substitute.For<ILanguageRepository>();

        _handler = new GetLanguageQueryHandler(_languageRepository);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_Should_Pass(int languageId)
    {
        var query = new GetLanguageQuery(languageId);
        var language = Language.Create("EN");
        _languageRepository.GetByIdAsync(query.LanguageId, CancellationToken.None).Returns(language);

        var result = await _handler.Handle(query, CancellationToken.None);

        await _languageRepository.Received(1).GetByIdAsync(query.LanguageId, CancellationToken.None);
        result.Should().BeOfType<GetLanguageQueryResponse>();
        result.Id.Should().Be(language.Id);
        result.Code.Should().Be(language.Code);
        result.CreatedAt.Should().Be(language.CreatedAt);
        result.UpdatedAt.Should().Be(language.UpdatedAt);
    }
}
