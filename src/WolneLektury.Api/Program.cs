using WolneLektury.Application.Authors.Dtos;
using WolneLektury.Application.Books.Dtos;
using WolneLektury.Application.Common.Pagination;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// --- Books ---
app.MapGet("/api/books", (
        int? page,
        int? pageSize,
        string? kind,
        string? genre,
        string? epoch,
        string? sortBy,
        string? sortDir) =>
    {
        var paged = new PagedResponse<BookDto>(
            Items: [],
            Page: page ?? 1,
            PageSize: pageSize ?? 20,
            TotalCount: 0);
        return Results.Ok(paged);
    })
    .WithName("GetBooks");

app.MapGet("/api/books/{slug}", (string slug) =>
    {
        var book = new BookDto(
            Slug: slug,
            Title: "Sample",
            Description: null,
            Url: null,
            Thumbnail: null,
            Authors: []);
        return Results.Ok(book);
    })
    .WithName("GetBookBySlug");

// --- Authors ---
app.MapGet("/api/authors", (
        int? page,
        int? pageSize,
        string? sortBy,
        string? sortDir) =>
    {
        var paged = new PagedResponse<AuthorDto>(
            Items: [],
            Page: page ?? 1,
            PageSize: pageSize ?? 20,
            TotalCount: 0);
        return Results.Ok(paged);
    })
    .WithName("GetAuthors");

app.MapGet("/api/authors/{slug}/books", (
        string slug,
        int? page,
        int? pageSize,
        string? sortBy,
        string? sortDir) =>
    {
        var paged = new PagedResponse<BookDto>(
            Items: [],
            Page: page ?? 1,
            PageSize: pageSize ?? 20,
            TotalCount: 0);
        return Results.Ok(paged);
    })
    .WithName("GetBooksByAuthor");

app.Run();
