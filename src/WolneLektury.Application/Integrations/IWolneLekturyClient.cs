using WolneLektury.Application.Authors.Dtos;
using WolneLektury.Application.Books.Dtos;

namespace WolneLektury.Application.Integrations;

/// <summary>
/// Upstream client for https://wolnelektury.pl/api/
/// </summary>
public interface IWolneLekturyClient
{
    Task<BookDto?> GetBookAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookDto>> GetBooksAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuthorDto>> GetAuthorsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookDto>> GetBooksByAuthorAsync(string authorSlug, CancellationToken cancellationToken = default);
}
