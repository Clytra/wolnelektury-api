using WolneLektury.Application.Authors.Queries;
using WolneLektury.Application.Books.Queries;
using WolneLektury.Application.Common.Sorting;

namespace WolneLektury.Api.Common;

public static class SortParsing
{
    public static SortDirection ParseSortDirection(string? sortDir) =>
        string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase) ? SortDirection.Desc : SortDirection.Asc;

    public static BookSortBy? ParseBookSortBy(string? sortBy) =>
        Enum.TryParse<BookSortBy>(sortBy, ignoreCase: true, out var value) ? value : null;

    public static AuthorSortBy ParseAuthorSortBy(string? sortBy) =>
        Enum.TryParse<AuthorSortBy>(sortBy, ignoreCase: true, out var value) ? value : AuthorSortBy.Name;
}
