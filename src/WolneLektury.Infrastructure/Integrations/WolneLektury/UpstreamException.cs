namespace WolneLektury.Infrastructure.Integrations.WolneLektury;

public class UpstreamException : Exception
{
    public int StatusCode { get; }

    public UpstreamException(int statusCode, string? message = null)
        : base(message ?? $"Upstream returned {statusCode}")
    {
        StatusCode = statusCode;
    }
}
