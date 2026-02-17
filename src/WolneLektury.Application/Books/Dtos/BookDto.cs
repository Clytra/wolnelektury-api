using WolneLektury.Application.Authors.Dtos;

namespace WolneLektury.Application.Books.Dtos;

public record BookDto(
    string Slug,
    string Title,
    string? Description,
    string? Url,
    string? Thumbnail,
    IReadOnlyList<AuthorDto> Authors);
