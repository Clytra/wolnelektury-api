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

Responses use `PagedResponse<T>` for list endpoints (e.g. `Items`, `Page`, `PageSize`, `TotalCount`, `TotalPages`).

## Tests

```powershell
# Run tests 
# dotnet test
```

## License

See repository license.
