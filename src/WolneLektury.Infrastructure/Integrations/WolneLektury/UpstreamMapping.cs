using WolneLektury.Application.Authors.Dtos;
using WolneLektury.Application.Books.Dtos;

namespace WolneLektury.Infrastructure.Integrations.WolneLektury;

public static class UpstreamMapping
{
    public static AuthorDto ToAuthorDto(UpstreamAuthorDto a) => new(a.Slug, a.Name);

    public static BookDto ToBookDto(UpstreamBookDto b)
    {
        var slug = b.Slug ?? SlugFromHref(b.Href) ?? b.Title;
        var authors = (b.Authors ?? [])
            .Select(a => new AuthorDto(a.Slug ?? SlugFromHref(a.Href) ?? "unknown", a.Name ?? ""))
            .ToList<AuthorDto>();
        return new BookDto(
            Slug: slug,
            Title: b.Title,
            Description: b.Description,
            Url: b.Url ?? b.Href,
            Thumbnail: b.CoverThumb,
            Authors: authors,
            Kind: b.Kind,
            Genre: b.Genre,
            Epoch: b.Epoch);
    }

    private static string? SlugFromHref(string? href)
    {
        if (string.IsNullOrEmpty(href)) return null;
        var segment = href.TrimEnd('/').Split('/').LastOrDefault();
        return string.IsNullOrEmpty(segment) ? null : segment;
    }
}
