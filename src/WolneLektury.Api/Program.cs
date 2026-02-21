using WolneLektury.Api.Endpoints;
using WolneLektury.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerDocs();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseGlobalExceptionHandling(app.Environment);
app.UseSwaggerDocsIfDevelopment();

app.MapHealthEndpoints();
app.MapBooksEndpoints();
app.MapAuthorsEndpoints();

app.Run();
