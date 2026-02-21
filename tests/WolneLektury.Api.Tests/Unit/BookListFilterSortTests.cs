using WolneLektury.Application.Authors.Dtos;
using WolneLektury.Application.Books.Dtos;
using WolneLektury.Application.Books.Queries;
using WolneLektury.Application.Common.Sorting;

namespace WolneLektury.Api.Tests.Unit;

public class BookListFilterSortTests
{
    private static BookDto Book(string slug, string title, string? kind = null, string? genre = null,
        string? epoch = null, string authorName = "Author") =>
        new(slug, title, null, null, null, [new AuthorDto("a", authorName)], kind, genre, epoch);

    [Fact]
    public void Apply_FiltersByKind()
    {
        var books = new[] { Book("a", "A", kind: "epika"), Book("b", "B", kind: "liryka"), Book("c", "C", kind: "epika") };
        var result = BookListFilterSort.Apply(books, kind: "epika", null, null, null, SortDirection.Asc);
        Assert.Equal(2, result.Count);
        Assert.All(result, b => Assert.Equal("epika", b.Kind));
    }

    [Fact]
    public void Apply_SortsByTitle_Asc()
    {
        var books = new[] { Book("z", "Z"), Book("a", "A"), Book("m", "M") };
        var result = BookListFilterSort.Apply(books, null, null, null, BookSortBy.Title, SortDirection.Asc);
        Assert.Equal(["A", "M", "Z"], result.Select(b => b.Title).ToArray());
    }

    [Fact]
    public void Apply_SortsByTitle_Desc()
    {
        var books = new[] { Book("a", "A"), Book("z", "Z"), Book("m", "M") };
        var result = BookListFilterSort.Apply(books, null, null, null, BookSortBy.Title, SortDirection.Desc);
        Assert.Equal(["Z", "M", "A"], result.Select(b => b.Title).ToArray());
    }

    [Fact]
    public void Apply_SortsByAuthorName_Asc()
    {
        var books = new[] { Book("x", "X", authorName: "C"), Book("y", "Y", authorName: "A"), Book("z", "Z", authorName: "B") };
        var result = BookListFilterSort.Apply(books, null, null, null, BookSortBy.AuthorName, SortDirection.Asc);
        Assert.Equal(["A", "B", "C"], result.Select(b => b.Authors[0].Name).ToArray());
    }
}
