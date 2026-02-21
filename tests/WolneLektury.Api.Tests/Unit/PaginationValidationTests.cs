using WolneLektury.Api.Common;

namespace WolneLektury.Api.Tests.Unit;

public class PaginationValidationTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 20)]
    [InlineData(1, 100)]
    [InlineData(5, 50)]
    public void Validate_ValidPageAndPageSize_ReturnsNull(int page, int pageSize)
    {
        var result = PaginationValidation.Validate(page, pageSize);
        Assert.Null(result);
    }

    [Fact]
    public void Validate_PageLessThanOne_ReturnsProblemDetails_WithPageError()
    {
        var result = PaginationValidation.Validate(0, 20);
        Assert.NotNull(result);
        Assert.Equal(400, result.Status);
        Assert.True(result.Extensions.TryGetValue("errors", out var errorsObj));
        var errors = Assert.IsType<Dictionary<string, string[]>>(errorsObj);
        Assert.True(errors.ContainsKey("page"));
        Assert.Contains("at least 1", errors["page"][0]);
    }

    [Fact]
    public void Validate_PageSizeTooLarge_ReturnsProblemDetails_WithPageSizeError()
    {
        var result = PaginationValidation.Validate(1, 101);
        Assert.NotNull(result);
        Assert.Equal(400, result.Status);
        Assert.True(result.Extensions.TryGetValue("errors", out var errorsObj));
        var errors = Assert.IsType<Dictionary<string, string[]>>(errorsObj);
        Assert.True(errors.ContainsKey("pageSize"));
        Assert.Contains("1 and 100", errors["pageSize"][0]);
    }

    [Fact]
    public void Validate_PageSizeZero_ReturnsProblemDetails_WithPageSizeError()
    {
        var result = PaginationValidation.Validate(1, 0);
        Assert.NotNull(result);
        Assert.Equal(400, result.Status);
        Assert.True(result.Extensions.TryGetValue("errors", out var errorsObj));
        var errors = Assert.IsType<Dictionary<string, string[]>>(errorsObj);
        Assert.True(errors.ContainsKey("pageSize"));
    }

    [Fact]
    public void Validate_BothInvalid_ReturnsProblemDetails_WithPageAndPageSizeErrors()
    {
        var result = PaginationValidation.Validate(0, 200);
        Assert.NotNull(result);
        Assert.Equal(400, result.Status);
        Assert.True(result.Extensions.TryGetValue("errors", out var errorsObj));
        var errors = Assert.IsType<Dictionary<string, string[]>>(errorsObj);
        Assert.True(errors.ContainsKey("page"));
        Assert.True(errors.ContainsKey("pageSize"));
    }

    [Theory]
    [InlineData(null, null, 1, 20)]
    [InlineData(1, 20, 1, 20)]
    [InlineData(2, 10, 2, 10)]
    [InlineData(null, 50, 1, 50)]
    [InlineData(5, null, 5, 20)]
    public void ApplyDefaults_ReturnsExpectedPageAndPageSize(int? page, int? pageSize, int expectedPage, int expectedPageSize)
    {
        var (p, ps) = PaginationValidation.ApplyDefaults(page, pageSize);
        Assert.Equal(expectedPage, p);
        Assert.Equal(expectedPageSize, ps);
    }
}
