using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Shared.Infrastructure;

namespace Test.Shared.Infrastructure;

[TestFixture]
public class HttpClientHelperTests
{
    [SetUp]
    public void SetUp()
    {
        _httpClientMock = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_httpClientMock.Object);
        _httpClientHelper = new HttpClientHelper(httpClient)
        {
            BaseAddress = "https://aaa.com"
        };
    }

    private HttpClientHelper _httpClientHelper = null!;
    private Mock<HttpMessageHandler> _httpClientMock = null!;

    [Test]
    public async Task PostAsync_WhenCalled_ReturnsHttpResponseMessage()
    {
        _httpClientMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage());

        var result = await _httpClientHelper.PostAsync("c");

        Assert.That(result, Is.TypeOf<HttpResponseMessage>());
    }

    [Test]
    public async Task PostAsync_WhenCalled_ReturnsCallsPostAsync()
    {
        _httpClientMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage());

        await _httpClientHelper.PostAsync("c");

        _httpClientMock.Protected()
        .Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>());
    }
}