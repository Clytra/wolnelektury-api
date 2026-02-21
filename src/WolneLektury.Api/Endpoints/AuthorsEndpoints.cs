using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using WolneLektury.Api.Common;
using WolneLektury.Application.Authors.Dtos;
using WolneLektury.Application.Authors.Queries;
using WolneLektury.Application.Books.Dtos;
using WolneLektury.Application.Books.Queries;
using WolneLektury.Application.Common.Pagination;
using WolneLektury.Application.Integrations;

namespace WolneLektury.Api.Endpoints;

public static class AuthorsEndpoints
{
    public static IEndpointRouteBuilder MapAuthorsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api")
            .WithTags("Authors");

        group.MapGet("/authors", async (
                int? page,
                int? pageSize,
                string? sortBy,
                string? sortDir,
                [FromServices] IWolneLekturyClient client) =>
            {
                var (p, ps) = PaginationValidation.ApplyDefaults(page, pageSize);
                var problem = PaginationValidation.Validate(p, ps);
                if (problem is not null)
                    return Results.Problem(problem);

                var sort = SortParsing.ParseAuthorSortBy(sortBy);
                var sortDirEnum = SortParsing.ParseSortDirection(sortDir);

                var all = await client.GetAuthorsAsync();
                var sorted = AuthorListSort.Apply(all, sort, sortDirEnum);
                var response = ListPagination.Page(sorted, p, ps);

                return Results.Ok(response);
            })
            .WithName("GetAuthors")
            .Produces<PagedResponse<AuthorDto>>(StatusCodes.Status200OK)
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

        group.MapGet("/authors/{slug}/books", async (
                string slug,
                int? page,
                int? pageSize,
                string? sortBy,
                string? sortDir,
                [FromServices] IWolneLekturyClient client) =>
            {
                var (p, ps) = PaginationValidation.ApplyDefaults(page, pageSize);
                var problem = PaginationValidation.Validate(p, ps);
                if (problem is not null)
                    return Results.Problem(problem);

                var sort = SortParsing.ParseBookSortBy(sortBy);
                var sortDirEnum = SortParsing.ParseSortDirection(sortDir);

                var books = await client.GetBooksByAuthorAsync(slug);
                var filteredSorted = BookListFilterSort.Apply(books, null, null, null, sort, sortDirEnum);
                var response = ListPagination.Page(filteredSorted, p, ps);

                return Results.Ok(response);
            })
            .WithName("GetBooksByAuthor")
            .Produces<PagedResponse<BookDto>>(StatusCodes.Status200OK)
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
