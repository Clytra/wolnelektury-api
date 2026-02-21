using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using WolneLektury.Infrastructure.Integrations.WolneLektury;

namespace WolneLektury.Api.Tests.Unit;

public class WolneLekturyClientTests
{
    private static HttpClient CreateClient(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
    {
        var messageHandler = new MockHttpMessageHandler(handler);
        return new HttpClient(messageHandler)
        {
            BaseAddress = new Uri("https://wolnelektury.pl/api/", UriKind.Absolute)
        };
    }

    [Fact]
    public async Task GetBookAsync_200_ReturnsMappedBook()
    {
        var upstream = new UpstreamBookDto(Title: "Pan Tadeusz", Slug: "pan-tadeusz");
        var json = JsonSerializer.Serialize(upstream);
        var client = CreateClient((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) }));
        var sut = new WolneLekturyApiClient(client, NullLogger<WolneLekturyApiClient>.Instance);

        var result = await sut.GetBookAsync("pan-tadeusz");

        Assert.NotNull(result);
        Assert.Equal("pan-tadeusz", result.Slug);
        Assert.Equal("Pan Tadeusz", result.Title);
    }

    [Fact]
    public async Task GetBookAsync_404_ReturnsNull()
    {
        var client = CreateClient((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)));
        var sut = new WolneLekturyApiClient(client, NullLogger<WolneLekturyApiClient>.Instance);

        var result = await sut.GetBookAsync("nonexistent");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetBookAsync_500_ThrowsUpstreamException()
    {
        var client = CreateClient((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("error") }));
        var sut = new WolneLekturyApiClient(client, NullLogger<WolneLekturyApiClient>.Instance);

        var ex = await Assert.ThrowsAsync<UpstreamException>(() => sut.GetBookAsync("any"));

        Assert.Equal(500, ex.StatusCode);
    }

    private sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;

        public MockHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler) => _handler = handler;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            _handler(request, cancellationToken);
    }
}

