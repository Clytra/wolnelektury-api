namespace WolneLektury.Application.Common.Pagination;

public record PagedResponse<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int? Total = null,
    bool? HasNext = null);
