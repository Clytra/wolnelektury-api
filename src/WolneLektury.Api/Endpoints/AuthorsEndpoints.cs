using Microsoft.OpenApi.Models;
using WolneLektury.Api.Common;
using WolneLektury.Application.Authors.Dtos;
using WolneLektury.Application.Authors.Queries;
using WolneLektury.Application.Books.Dtos;
using WolneLektury.Application.Common.Pagination;

namespace WolneLektury.Api.Endpoints;

public static class AuthorsEndpoints
{
    public static IEndpointRouteBuilder MapAuthorsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api")
            .WithTags("Authors");

        group.MapGet("/authors", (
                int? page,
                int? pageSize,
                string? sortBy,
                string? sortDir) =>
            {
                var (p, ps) = PaginationValidation.ApplyDefaults(page, pageSize);
                var problem = PaginationValidation.Validate(p, ps);
                if (problem is not null)
                    return Results.Problem(problem);

                var _ = new AuthorListQuery(
                    new PaginationParams(p, ps),
                    SortParsing.ParseAuthorSortBy(sortBy),
                    SortParsing.ParseSortDirection(sortDir));

                var response = new PagedResponse<AuthorDto>([], p, ps, Total: 0, HasNext: false);
                return Results.Json(response, statusCode: StatusCodes.Status501NotImplemented);
            })
            .WithName("GetAuthors")
            .Produces<PagedResponse<AuthorDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status501NotImplemented)
            .ProducesProblem(StatusCodes.Status400BadRequest, contentType: "application/problem+json")
            .ProducesProblem(StatusCodes.Status500InternalServerError, contentType: "application/problem+json")
            .WithSummary("List authors")
            .WithDescription("Returns a paginated list of authors. Supports sorting by name.")
            .WithOpenApi(op =>
            {
                Describe(op, "page", "Page number (>= 1). Default: 1.");
                Describe(op, "pageSize", "Items per page (1-100). Default: 20.");
                Describe(op, "sortBy", "Sort field. Allowed: name. Default: name.");
                Describe(op, "sortDir", "Sort direction. Allowed: asc|desc. Default: asc.");
                return op;
            });

        group.MapGet("/authors/{slug}/books", (
                string slug,
                int? page,
                int? pageSize,
                string? sortBy,
                string? sortDir) =>
            {
                var (p, ps) = PaginationValidation.ApplyDefaults(page, pageSize);
                var problem = PaginationValidation.Validate(p, ps);
                if (problem is not null)
                    return Results.Problem(problem);

                var response = new PagedResponse<BookDto>([], p, ps, Total: 0, HasNext: false);
                return Results.Json(response, statusCode: StatusCodes.Status501NotImplemented);
            })
            .WithName("GetBooksByAuthor")
            .Produces<PagedResponse<BookDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status501NotImplemented)
            .ProducesProblem(StatusCodes.Status400BadRequest, contentType: "application/problem+json")
            .ProducesProblem(StatusCodes.Status500InternalServerError, contentType: "application/problem+json")
            .WithSummary("List books by author")
            .WithDescription("Returns a paginated list of books for the given author slug. Example slug: adam-mickiewicz.")
            .WithOpenApi(op =>
            {
                Describe(op, "slug", "Author slug (ID). Example: adam-mickiewicz.");
                Describe(op, "page", "Page number (>= 1). Default: 1.");
                Describe(op, "pageSize", "Items per page (1-100). Default: 20.");
                Describe(op, "sortBy", "Sort field. Allowed: title|authorName. Default: title.");
                Describe(op, "sortDir", "Sort direction. Allowed: asc|desc. Default: asc.");
                return op;
            });

        return app;
    }

    private static void Describe(OpenApiOperation op, string name, string description)
    {
        var p = op.Parameters.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
        if (p is not null)
            p.Description = description;
    }
}
