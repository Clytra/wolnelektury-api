# Wolne Lektury API

REST API for Wolne Lektury (target .NET 8).

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (version pinned in `global.json`)

## Getting started

```powershell
# Restore and build
dotnet restore
dotnet build

# Run 
# dotnet run --project src/WolneLektury.Api
```

Then open:

- **API**: http://localhost:5046 (or the port shown in the console)
- **Swagger UI**: http://localhost:5046/swagger

## Endpoints

| Method | Route | Query parameters |
|--------|--------|-------------------|
| GET | `/api/books` | `page`, `pageSize`, `kind`, `genre`, `epoch`, `sortBy`, `sortDir` |
| GET | `/api/books/{slug}` | — |
| GET | `/api/authors` | `page`, `pageSize`, `sortBy` (default: name), `sortDir` |
| GET | `/api/authors/{slug}/books` | `page`, `pageSize`, `sortBy`, `sortDir` |

List endpoints return PagedResponse<T> with: items, page, pageSize, and optionally total and hasNext.

## Tests

```powershell
# Run tests 
# dotnet test
```

## Example requests (curl)

```bash
# List books (first page, 10 items)
curl -s "http://localhost:5046/api/books?page=1&pageSize=10"

# Book by slug
curl -s "http://localhost:5046/api/books/pan-tadeusz"

# List authors
curl -s "http://localhost:5046/api/authors?page=1&pageSize=20"

# Books by author
curl -s "http://localhost:5046/api/authors/adam-mickiewicz/books?page=1&pageSize=10"

# Health
curl -s "http://localhost:5046/health"
```

## Assumptions & trade-offs

- **Upstream:** Data is fetched from [wolnelektury.pl API](https://wolnelektury.pl/api/) (authors, works). Paths and response shape are assumed from public docs; verify against live API if needed.
- **Pagination/filtering/sorting:** Applied in memory after fetching the full list from upstream. `Total` = count after filtering; `HasNext` = `page * pageSize < Total`. No server-side cursor or upstream pagination.
- **Errors:** 404 for missing book/author returns RFC 7807 ProblemDetails. Upstream failures (5xx) are mapped to 502/503 with `traceId` in the response for support.
- **Tests:** Integration tests call the real API (no upstream mock in WebApplicationFactory); unit tests cover mapping, filter/sort, and client with mocked `HttpMessageHandler`.

## License

See repository license.
