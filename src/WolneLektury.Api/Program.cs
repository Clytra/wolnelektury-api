using WolneLektury.Api.Endpoints;
using WolneLektury.Api.Extensions;
using WolneLektury.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWolneLekturyClient();
builder.Services.AddSwaggerDocs();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseGlobalExceptionHandling(app.Environment);
app.UseSwaggerDocsIfDevelopment();

app.MapHealthEndpoints();
app.MapBooksEndpoints();
app.MapAuthorsEndpoints();

app.Run();
