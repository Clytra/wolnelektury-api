using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using WolneLektury.Api.Common;
using WolneLektury.Application.Books.Dtos;
using WolneLektury.Application.Books.Queries;
using WolneLektury.Application.Common.Pagination;
using WolneLektury.Application.Integrations;

namespace WolneLektury.Api.Endpoints;

public static class BooksEndpoints
{
    public static IEndpointRouteBuilder MapBooksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api")
            .WithTags("Books");

        group.MapGet("/books", async (
                int? page,
                int? pageSize,
                string? kind,
                string? genre,
                string? epoch,
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

                var all = await client.GetBooksAsync();
                var filteredSorted = BookListFilterSort.Apply(all, kind, genre, epoch, sort, sortDirEnum);
                var response = ListPagination.Page(filteredSorted, p, ps);

                return Results.Ok(response);
            })
            .WithName("GetBooks")
            .Produces<PagedResponse<BookDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest, contentType: "application/problem+json")
            .ProducesProblem(StatusCodes.Status500InternalServerError, contentType: "application/problem+json")
            .WithSummary("List books")
            .WithDescription("Returns a paginated list of books. Supports filtering by kind/genre/epoch and sorting by title or author name.")
            .WithOpenApi(op =>
            {
                Describe(op, "page", "Page number (>= 1). Default: 1.");
                Describe(op, "pageSize", "Items per page (1-100). Default: 20.");
                Describe(op, "kind", "Filter by kind (upstream value).");
                Describe(op, "genre", "Filter by genre (upstream value).");
                Describe(op, "epoch", "Filter by epoch (upstream value).");
                Describe(op, "sortBy", "Sort field. Allowed: title|authorName. Default: title.");
                Describe(op, "sortDir", "Sort direction. Allowed: asc|desc. Default: asc.");
                return op;
            });

        group.MapGet("/books/{slug}", async (string slug, [FromServices] IWolneLekturyClient client) =>
            {
                var book = await client.GetBookAsync(slug);
                if (book is null)
                    return Results.Problem(statusCode: 404, title: "Not Found", detail: $"Book '{slug}' not found.");

                return Results.Ok(book);
            })
            .WithName("GetBookBySlug")
            .Produces<BookDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound, contentType: "application/problem+json")
            .ProducesProblem(StatusCodes.Status500InternalServerError, contentType: "application/problem+json")
            .WithSummary("Get book by slug")
            .WithDescription("Returns a single book by its slug (ID). Example slug: pan-tadeusz.")
            .WithOpenApi(op =>
            {
                Describe(op, "slug", "Book slug (ID). Example: pan-tadeusz.");
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
