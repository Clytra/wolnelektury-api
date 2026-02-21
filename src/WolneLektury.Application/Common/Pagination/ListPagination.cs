namespace WolneLektury.Application.Common.Pagination;

public static class ListPagination
{
    public static PagedResponse<T> Page<T>(
        IReadOnlyList<T> items,
        int page,
        int pageSize)
    {
        var total = items.Count;
        var skip = (page - 1) * pageSize;
        var paged = items.Skip(skip).Take(pageSize).ToList();
        var hasNext = page * pageSize < total;
        return new PagedResponse<T>(paged, page, pageSize, total, hasNext);
    }
}
