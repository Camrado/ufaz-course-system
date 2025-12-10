using CourseSystem.Application.Languages.UpdateLanguage;
using CourseSystem.Persistence.Languages;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Languages.Handlers;

public class UpdateLanguageHandlerTests
{
    private readonly UpdateLanguageCommandHandler _handler;
    private readonly ILanguageRepository _languageRepository;

    public UpdateLanguageHandlerTests()
    {
        _languageRepository = Substitute.For<ILanguageRepository>();

        _handler = new UpdateLanguageCommandHandler(_languageRepository);
    }

    [Theory]
    [InlineData(1, "Ru")]
    public async Task Handle_Should_Pass(int languageId, string code)
    {
        var command = new UpdateLanguageCommand { LanguageId = languageId, Code = code };
        var language = Language.Create("EN");
        _languageRepository.GetByIdAsync(command.LanguageId, CancellationToken.None).Returns(language);

        var result = await _handler.Handle(command, CancellationToken.None);

        await _languageRepository.Received(1).GetByIdAsync(command.LanguageId, CancellationToken.None);
        result.Should().BeOfType<UpdateLanguageCommandResponse>();
        result.Id.Should().Be(language.Id);
    }
}
