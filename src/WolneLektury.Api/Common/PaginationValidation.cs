using Microsoft.AspNetCore.Mvc;

namespace WolneLektury.Api.Common;

public static class PaginationValidation
{
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 100;

    /// <summary>
    /// Validates page (>= 1) and pageSize (1..100). Returns ProblemDetails if invalid.
    /// </summary>
    public static ProblemDetails? Validate(int page, int pageSize)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);

        if (page < 1)
            errors["page"] = ["Page must be at least 1."];

        if (pageSize is < 1 or > MaxPageSize)
            errors["pageSize"] = [$"PageSize must be between 1 and {MaxPageSize}."];

        if (errors.Count == 0)
            return null;

        var problem = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Extensions =
            {
                ["errors"] = errors
            }
        };
        return problem;
    }

    public static (int Page, int PageSize) ApplyDefaults(int? page, int? pageSize) =>
        (page ?? 1, pageSize ?? DefaultPageSize);
}
