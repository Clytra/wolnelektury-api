using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using WolneLektury.Application.Authors.Dtos;
using WolneLektury.Application.Books.Dtos;
using WolneLektury.Application.Integrations;

namespace WolneLektury.Infrastructure.Integrations.WolneLektury;

public sealed class WolneLekturyApiClient : IWolneLekturyClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WolneLekturyApiClient> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public WolneLekturyApiClient(HttpClient httpClient, ILogger<WolneLekturyApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<BookDto?> GetBookAsync(string slug, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"work/{slug}/", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        await EnsureSuccessAsync(response, cancellationToken);
        var dto = await ReadAsAsync<UpstreamBookDto>(response, cancellationToken);
        return dto is null ? null : UpstreamMapping.ToBookDto(dto);
    }

    public async Task<IReadOnlyList<BookDto>> GetBooksAsync(CancellationToken cancellationToken = default)
    {
        var list = await GetWorksListAsync(cancellationToken);
        return list.Select(UpstreamMapping.ToBookDto).ToList();
    }

    public async Task<IReadOnlyList<AuthorDto>> GetAuthorsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("authors/", cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        var list = await ReadAsAsync<List<UpstreamAuthorDto>>(response, cancellationToken);
        return (list ?? []).Select(UpstreamMapping.ToAuthorDto).ToList();
    }

    public async Task<IReadOnlyList<BookDto>> GetBooksByAuthorAsync(string authorSlug, CancellationToken cancellationToken = default)
    {
        var list = await GetWorksListAsync(cancellationToken);
        var filtered = list
            .Where(b => (b.Authors ?? []).Any(a => string.Equals(a.Slug, authorSlug, StringComparison.OrdinalIgnoreCase)))
            .Select(UpstreamMapping.ToBookDto)
            .ToList();
        return filtered;
    }

    private async Task<List<UpstreamBookDto>> GetWorksListAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync("works/", cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        var list = await ReadAsAsync<List<UpstreamBookDto>>(response, cancellationToken);
        return list ?? [];
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode) return;
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new UpstreamException((int)response.StatusCode, body);
    }

    private static async Task<T?> ReadAsAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }
}
