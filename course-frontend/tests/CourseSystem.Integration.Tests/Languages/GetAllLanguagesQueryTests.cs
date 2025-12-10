using System.Net;
using System.Net.Http.Json;
using CourseSystem.Application.Languages.GetAllLanguages;
using CourseSystem.Infrastructure;
using CourseSystem.Integration.Tests.Common;
using FluentAssertions;

namespace CourseSystem.Integration.Tests.Languages;

public class GetAllLanguagesQueryTests : IClassFixture<IntegrationTestFixture>
{
    private readonly HttpClient _client;

    public GetAllLanguagesQueryTests(IntegrationTestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Theory]
    [InlineData(LanguageConstants.RecordsCount,
        LanguageConstants.SampleLanguageId,
        LanguageConstants.SampleLanguageCode)]
    public async Task Should_Return_All_Languages_When_LanguagesExist(int expectedCount,
        int languageId,
        string languageName)
    {
        var response = await _client.GetAsync("/api/languages");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<GetAllLanguagesQueryResponse>();
        content.Should().NotBeNull();
        content.Languages.Should().HaveCount(expectedCount);
        content.Languages.Should().Contain(l => l.Id == languageId && l.Code == languageName);
    }
}
