using WolneLektury.Application.Authors.Dtos;
using WolneLektury.Application.Authors.Queries;
using WolneLektury.Application.Common.Sorting;

namespace WolneLektury.Api.Tests.Unit;

public class AuthorListSortTests
{
    [Fact]
    public void Apply_SortsByName_Asc()
    {
        var authors = new[] { new AuthorDto("z", "Z"), new AuthorDto("a", "A"), new AuthorDto("m", "M") };
        var result = AuthorListSort.Apply(authors, AuthorSortBy.Name, SortDirection.Asc);
        Assert.Equal(["A", "M", "Z"], result.Select(a => a.Name).ToArray());
    }

    [Fact]
    public void Apply_SortsByName_Desc()
    {
        var authors = new[] { new AuthorDto("a", "A"), new AuthorDto("z", "Z"), new AuthorDto("m", "M") };
        var result = AuthorListSort.Apply(authors, AuthorSortBy.Name, SortDirection.Desc);
        Assert.Equal(["Z", "M", "A"], result.Select(a => a.Name).ToArray());
    }

    [Fact]
    public void Apply_DefaultSortBy_Name_Asc()
    {
        var authors = new[] { new AuthorDto("b", "B"), new AuthorDto("a", "A") };
        var result = AuthorListSort.Apply(authors, null, SortDirection.Asc);
        Assert.Equal(["A", "B"], result.Select(a => a.Name).ToArray());
    }
}
