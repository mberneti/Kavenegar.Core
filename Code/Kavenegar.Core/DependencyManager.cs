using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;

namespace Kavenegar.Core;

public static class DependencyManager
{
    public static IServiceCollection AddKavenegar(
        this IServiceCollection serviceCollection,
        string apiKey)
    {
        return serviceCollection.AddScoped<IHttpClientHelper, HttpClientHelper>()
            .AddScoped<IKavenegarProfileApi, KavenegarProfileApi>(
                serviceProvider => new KavenegarProfileApi(
                    serviceProvider.GetRequiredService<IHttpClientHelper>(),
                    apiKey))
            .AddScoped<IKavenegarMessageSender, KavenegarMessageSender>(
                serviceProvider => new KavenegarMessageSender(
                    serviceProvider.GetRequiredService<IHttpClientHelper>(),
                    apiKey));
    }
}