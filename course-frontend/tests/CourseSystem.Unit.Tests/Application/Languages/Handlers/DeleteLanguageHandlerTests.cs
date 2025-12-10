using CourseSystem.Application.Languages.DeleteLanguage;
using CourseSystem.Persistence.Languages;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Languages.Handlers;

public class DeleteLanguageHandlerTests
{
    private readonly DeleteLanguageCommandHandler _handler;
    private readonly ILanguageRepository _languageRepository;

    public DeleteLanguageHandlerTests()
    {
        _languageRepository = Substitute.For<ILanguageRepository>();

        _handler = new DeleteLanguageCommandHandler(_languageRepository);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_Should_Pass(int languageId)
    {
        var command = new DeleteLanguageCommand(languageId);
        var language = Language.Create("EN");
        _languageRepository.GetByIdAsync(command.LanguageId, CancellationToken.None).Returns(language);

        var result = await _handler.Handle(command, CancellationToken.None);

        await _languageRepository.Received(1).GetByIdAsync(command.LanguageId, CancellationToken.None);
        _languageRepository.Received(1).Delete(language, CancellationToken.None);
        result.Should().BeOfType<DeleteLanguageCommandResponse>();
    }
}
