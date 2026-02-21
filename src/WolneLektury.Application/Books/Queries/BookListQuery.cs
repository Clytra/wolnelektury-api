using WolneLektury.Application.Common.Pagination;
using WolneLektury.Application.Common.Sorting;

namespace WolneLektury.Application.Books.Queries;

public record BookListQuery(
    PaginationParams Pagination,
    string? Kind = null,
    string? Genre = null,
    string? Epoch = null,
    BookSortBy? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc);
