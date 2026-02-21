using WolneLektury.Application.Authors.Dtos;
using WolneLektury.Application.Common.Sorting;

namespace WolneLektury.Application.Authors.Queries;

public static class AuthorListSort
{
    public static IReadOnlyList<AuthorDto> Apply(
        IReadOnlyList<AuthorDto> authors,
        AuthorSortBy? sortBy,
        SortDirection sortDirection)
    {
        var q = authors.AsEnumerable();
        q = (sortBy ?? AuthorSortBy.Name) == AuthorSortBy.Name && sortDirection == SortDirection.Desc
            ? q.OrderByDescending(a => a.Name)
            : q.OrderBy(a => a.Name);
        return q.ToList();
    }
}
