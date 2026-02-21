using WolneLektury.Application.Books.Dtos;
using WolneLektury.Application.Common.Sorting;

namespace WolneLektury.Application.Books.Queries;

public static class BookListFilterSort
{
    public static IReadOnlyList<BookDto> Apply(
        IReadOnlyList<BookDto> books,
        string? kind,
        string? genre,
        string? epoch,
        BookSortBy? sortBy,
        SortDirection sortDirection)
    {
        var q = books.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(kind))
            q = q.Where(b => string.Equals(b.Kind, kind, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(genre))
            q = q.Where(b => string.Equals(b.Genre, genre, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(epoch))
            q = q.Where(b => string.Equals(b.Epoch, epoch, StringComparison.OrdinalIgnoreCase));

        q = sortBy switch
        {
            BookSortBy.AuthorName => sortDirection == SortDirection.Desc
                ? q.OrderByDescending(b => b.Authors.FirstOrDefault()?.Name ?? "")
                : q.OrderBy(b => b.Authors.FirstOrDefault()?.Name ?? ""),
            _ => sortDirection == SortDirection.Desc
                ? q.OrderByDescending(b => b.Title)
                : q.OrderBy(b => b.Title)
        };
        return q.ToList();
    }
}
