using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Shared.Infrastructure;

namespace Test.Shared.Infrastructure;

[TestFixture]
public class JsonUtilityTests
{
    [Test]
    public void Serialize_WhenCalled_ReturnsObjectJson()
    {
        var result = new object().Serialize();

        Assert.That(result, Is.EqualTo("{}"));
    }

    [Test]
    public async Task Deserialize_WhenCalled_ReturnsDeserializedObject()
    {
        var httpResponseMessage = new HttpResponseMessage
        {
            Content = new StringContent("[{}]")
        };

        var result = await httpResponseMessage.Deserialize<object[]>();

        Assert.That(result, Is.TypeOf<object[]>());
    }
}