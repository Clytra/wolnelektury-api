using WolneLektury.Application.Common.Pagination;

namespace WolneLektury.Api.Tests.Unit;

public class ListPaginationTests
{
    [Fact]
    public void Page_EmptyList_ReturnsZeroTotal_AndNoItems()
    {
        var result = ListPagination.Page(Array.Empty<int>(), 1, 20);
        Assert.Empty(result.Items);
        Assert.Equal(1, result.Page);
        Assert.Equal(20, result.PageSize);
        Assert.Equal(0, result.Total);
        Assert.False(result.HasNext);
    }

    [Fact]
    public void Page_FirstPage_ReturnsHasNextWhenMoreExist()
    {
        var items = Enumerable.Range(1, 25).ToList();
        var result = ListPagination.Page(items, 1, 10);
        Assert.Equal(10, result.Items.Count);
        Assert.Equal(25, result.Total);
        Assert.True(result.HasNext!.Value);
    }

    [Fact]
    public void Page_LastPage_ReturnsNoHasNext()
    {
        var items = Enumerable.Range(1, 25).ToList();
        var result = ListPagination.Page(items, 3, 10);
        Assert.Equal(5, result.Items.Count);
        Assert.Equal(25, result.Total);
        Assert.False(result.HasNext!.Value);
    }
}
