using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using WolneLektury.Application.Books.Dtos;
using WolneLektury.Application.Common.Pagination;

namespace WolneLektury.Api.Tests.Integration;

public class BooksEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BooksEndpointTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetBooks_Returns200_AndValidPagedResponseShape()
    {
        var response = await _client.GetAsync("/api/books?page=1&pageSize=5");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var paged = await response.Content.ReadFromJsonAsync<PagedResponse<BookDto>>();
        Assert.NotNull(paged);
        Assert.NotNull(paged.Items);
        Assert.Equal(1, paged.Page);
        Assert.Equal(5, paged.PageSize);
        Assert.NotNull(paged.Total);
        Assert.NotNull(paged.HasNext);
    }

    [Fact]
    public async Task GetBookBySlug_InvalidSlug_Returns404_WithProblemDetails()
    {
        var response = await _client.GetAsync("/api/books/nonexistent-slug-12345");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(404, problem.Status);
        Assert.Contains("not found", problem.Detail ?? "");
    }
}
