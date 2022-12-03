using Microsoft.Extensions.DependencyInjection;

namespace Kavenegar.Core;

public static class DependencyManager
{
    public static IServiceCollection AddKavenegar(
        this IServiceCollection serviceCollection,
        string apiKey)
    {
        return serviceCollection.AddTransient<IKavenegarApi, KavenegarApi>(_ => new KavenegarApi(apiKey));
    }
}