using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;

namespace Kavenegar.Core;

public static class DependencyManager
{
    public static IServiceCollection AddKavenegar(
        this IServiceCollection serviceCollection,
        string apiKey)
    {
        serviceCollection.AddScoped<HttpClient>();
        serviceCollection.AddScoped<IHttpClientHelper, HttpClientHelper>();

        serviceCollection.AddScoped<IKavenegarProfileApi, KavenegarProfileApi>(
            serviceProvider => new KavenegarProfileApi(
                serviceProvider.GetRequiredService<IHttpClientHelper>(),
                apiKey));

        serviceCollection.AddScoped<IKavenegarMessageSender, KavenegarMessageSender>(
            serviceProvider => new KavenegarMessageSender(
                serviceProvider.GetRequiredService<IHttpClientHelper>(),
                apiKey));

        return serviceCollection;
    }
}