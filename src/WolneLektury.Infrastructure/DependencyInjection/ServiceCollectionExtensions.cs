using Microsoft.Extensions.DependencyInjection;
using WolneLektury.Application.Integrations;
using WolneLektury.Infrastructure.Integrations.WolneLektury;

namespace WolneLektury.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWolneLekturyClient(this IServiceCollection services)
    {
        services.AddHttpClient<IWolneLekturyClient, WolneLekturyApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://wolnelektury.pl/api/", UriKind.Absolute);
            client.Timeout = TimeSpan.FromSeconds(15);
        });
        return services;
    }
}
