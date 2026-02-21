using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace WolneLektury.Api.Tests.Integration;

/// <summary>
/// API contract tests: pagination validation returns 400 and application/problem+json with errors.
/// </summary>
public class BooksEndpointPaginationValidationTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BooksEndpointPaginationValidationTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetBooks_InvalidPage_Returns400_WithProblemDetails_AndPageError()
    {
        var response = await _client.GetAsync("/api/books?page=0&pageSize=20");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);

        Assert.True(problem.Extensions.TryGetValue("errors", out var errorsObj), "Expected 'errors' in Extensions");
        var errors = Assert.IsType<JsonElement>(errorsObj);
        Assert.True(errors.TryGetProperty("page", out _), "Expected 'page' in errors");
    }

    [Fact]
    public async Task GetBooks_InvalidPageSize_Returns400_WithProblemDetails_AndPageSizeError()
    {
        var response = await _client.GetAsync("/api/books?page=1&pageSize=200");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);

        Assert.True(problem.Extensions.TryGetValue("errors", out var errorsObj), "Expected 'errors' in Extensions");
        var errors = Assert.IsType<JsonElement>(errorsObj);
        Assert.True(errors.TryGetProperty("pageSize", out _), "Expected 'pageSize' in errors");
    }
}
