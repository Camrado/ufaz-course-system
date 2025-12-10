using System.Net;
using System.Net.Http.Json;
using CourseSystem.Application.Categories.GetAllCategories;
using CourseSystem.Infrastructure;
using CourseSystem.Integration.Tests.Common;
using FluentAssertions;

namespace CourseSystem.Integration.Tests.Categories;

public class GetAllCategoriesQueryTests : IClassFixture<IntegrationTestFixture>
{
    private readonly HttpClient _client;

    public GetAllCategoriesQueryTests(IntegrationTestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Theory]
    [InlineData(CategoryConstants.RecordsCount,
        CategoryConstants.SampleCategoryId,
        CategoryConstants.SampleCategoryName)]
    public async Task Should_Return_All_Categories_When_CategoriesExist(int expectedCount,
        int categoryId,
        string categoryName)
    {
        var response = await _client.GetAsync("/api/categories");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<GetAllCategoriesQueryResponse>();
        content.Should().NotBeNull();
        content.Categories.Should().HaveCount(expectedCount);
        content.Categories.Should().Contain(c => c.Id == categoryId && c.Name == categoryName);
    }
}
