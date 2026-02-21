using WolneLektury.Application.Common.Pagination;
using WolneLektury.Application.Common.Sorting;

namespace WolneLektury.Application.Authors.Queries;

public record AuthorListQuery(
    PaginationParams Pagination,
    AuthorSortBy? SortBy = AuthorSortBy.Name,
    SortDirection SortDirection = SortDirection.Asc);
