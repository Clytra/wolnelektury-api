using System.Text.Json.Serialization;

namespace WolneLektury.Infrastructure.Integrations.WolneLektury;

public record UpstreamAuthorDto(
    [property: JsonPropertyName("slug")] string Slug,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("url")] string? Url = null,
    [property: JsonPropertyName("href")] string? Href = null);

public record UpstreamBookDto(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("slug")] string? Slug = null,
    [property: JsonPropertyName("url")] string? Url = null,
    [property: JsonPropertyName("href")] string? Href = null,
    [property: JsonPropertyName("description")] string? Description = null,
    [property: JsonPropertyName("cover_thumb")] string? CoverThumb = null,
    [property: JsonPropertyName("authors")] List<UpstreamAuthorRef>? Authors = null,
    [property: JsonPropertyName("kind")] string? Kind = null,
    [property: JsonPropertyName("genre")] string? Genre = null,
    [property: JsonPropertyName("epoch")] string? Epoch = null);

public record UpstreamAuthorRef(
    [property: JsonPropertyName("slug")] string? Slug,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("href")] string? Href);
