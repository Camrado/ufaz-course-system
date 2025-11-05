using CourseSystem.Application.Languages.CreateLanguage;
using CourseSystem.Persistence.Languages;
using FluentAssertions;
using NSubstitute;

namespace CourseSystem.Unit.Tests.Application.Languages.Handlers;

public class CreateLanguageHandlerTests
{
    private readonly CreateLanguageCommandHandler _handler;
    private readonly ILanguageRepository _languageRepository;

    public CreateLanguageHandlerTests()
    {
        _languageRepository = Substitute.For<ILanguageRepository>();

        _handler = new CreateLanguageCommandHandler(_languageRepository);
    }

    [Theory]
    [InlineData("EN")]
    public async Task Handle_Should_Pass(string languageCode)
    {
        var command = new CreateLanguageCommand(languageCode);

        var result = await _handler.Handle(command, CancellationToken.None);

        _languageRepository.Received(1).Add(Arg.Is<Language>(l => l.Code == command.Code), CancellationToken.None);
        result.Should().BeOfType<CreateLanguageCommandResponse>();
        result.Id.Should().Be(0); // TODO: Adjust it in future to return the actual ID
    }
}
