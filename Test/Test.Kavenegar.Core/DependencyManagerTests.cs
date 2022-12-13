using System.Linq;
using System.Net.Http;
using Kavenegar.Core;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Infrastructure;

namespace Test.Kavenegar.Core;

[TestFixture]
public class DependencyManagerTests
{
    [SetUp]
    public void SetUp()
    {
        _serviceCollection = new ServiceCollection().AddKavenegar("apiKey");
    }

    private IServiceCollection _serviceCollection = null!;

    [Test]
    public void AddKavenegar_WhenCalled_IKavenegarProfileApiServiceAdded()
    {
        var serviceExistence = _serviceCollection.Any(
            i => i.Lifetime == ServiceLifetime.Scoped &&
                 i.ServiceType == typeof(IKavenegarProfileApi) &&
                 i.ImplementationFactory!.Method.ReturnType == typeof(global::Kavenegar.Core.KavenegarProfileApi));

        Assert.That(serviceExistence, Is.True);
    }

    [Test]
    public void AddKavenegar_WhenCalled_IKavenegarMessageSenderServiceAdded()
    {
        var serviceExistence = _serviceCollection.Any(
            i => i.Lifetime == ServiceLifetime.Scoped &&
                 i.ServiceType == typeof(IKavenegarMessageSender) &&
                 i.ImplementationFactory!.Method.ReturnType == typeof(global::Kavenegar.Core.KavenegarMessageSender));

        Assert.That(serviceExistence, Is.True);
    }

    [Test]
    public void AddKavenegar_WhenCalled_IHttpClientHelperServiceAdded()
    {
        var serviceExistence = _serviceCollection.Any(
            i => i.Lifetime == ServiceLifetime.Scoped &&
                 i.ServiceType == typeof(IHttpClientHelper) &&
                 i.ImplementationType == typeof(HttpClientHelper));

        Assert.That(serviceExistence, Is.True);
    }

    [Test]
    public void AddKavenegar_WhenCalled_HttpClientServiceAdded()
    {
        var serviceExistence = _serviceCollection.Any(
            i => i.Lifetime == ServiceLifetime.Scoped &&
                 i.ServiceType == typeof(HttpClient) &&
                 i.ImplementationType == typeof(HttpClient));

        Assert.That(serviceExistence, Is.True);
    }

    [Test]
    public void AddKavenegar_WhenCalled_CheckBuiltServices()
    {
        var provider = _serviceCollection.BuildServiceProvider();

        Assert.That(
            provider.GetService<IKavenegarProfileApi>(),
            Is.TypeOf<global::Kavenegar.Core.KavenegarProfileApi>());

        Assert.That(
            provider.GetService<IKavenegarMessageSender>(),
            Is.TypeOf<global::Kavenegar.Core.KavenegarMessageSender>());

        Assert.That(provider.GetService<IHttpClientHelper>(), Is.TypeOf<HttpClientHelper>());

        Assert.That(provider.GetService<HttpClient>(), Is.TypeOf<HttpClient>());
    }
}