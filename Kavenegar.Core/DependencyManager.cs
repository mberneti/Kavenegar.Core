using Kavenegar.Core.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Kavenegar.Core;

public static class DependencyManager
{
    public static IServiceCollection AddKavenegar(
        this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddAutoMapper(typeof(MappingProfiles).Assembly);
    }
}