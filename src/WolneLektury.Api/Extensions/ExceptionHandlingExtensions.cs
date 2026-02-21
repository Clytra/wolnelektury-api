using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WolneLektury.Infrastructure.Integrations.WolneLektury;

namespace WolneLektury.Api.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(handler =>
        {
            handler.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                var error = feature?.Error;
                var traceId = context.TraceIdentifier;
                ProblemDetails problem;
                if (error is UpstreamException upstream)
                {
                    problem = ProblemDetailsHelpers.UpstreamFailure(upstream.StatusCode, traceId, upstream.Message);
                    context.Response.StatusCode = problem.Status ?? 502;
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    problem = new ProblemDetails
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                        Title = "An error occurred",
                        Status = context.Response.StatusCode,
                        Detail = error?.Message
                    };
                    if (!string.IsNullOrEmpty(traceId))
                        problem.Extensions["traceId"] = traceId;
                }
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problem);
            });
        });
        return app;
    }
}
