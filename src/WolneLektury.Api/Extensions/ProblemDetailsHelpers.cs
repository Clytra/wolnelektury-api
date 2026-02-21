using Microsoft.AspNetCore.Mvc;

namespace WolneLektury.Api.Extensions;

public static class ProblemDetailsHelpers
{
    private const string NotFoundType = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
    private const string ServerErrorType = "https://tools.ietf.org/html/rfc7231#section-6.6.1";

    public static ProblemDetails NotFound(string resourceType, string key)
    {
        return new ProblemDetails
        {
            Type = NotFoundType,
            Title = "Not Found",
            Status = StatusCodes.Status404NotFound,
            Detail = $"{resourceType} '{key}' not found."
        };
    }

    public static ProblemDetails UpstreamFailure(int statusCode, string? traceId, string? detail = null)
    {
        var status = statusCode is 503 or 504 ? 503 : 502;
        var problem = new ProblemDetails
        {
            Type = ServerErrorType,
            Title = status == 503 ? "Service Unavailable" : "Bad Gateway",
            Status = status,
            Detail = detail ?? "Upstream request failed."
        };
        if (!string.IsNullOrEmpty(traceId))
            problem.Extensions["traceId"] = traceId;
        return problem;
    }
}
