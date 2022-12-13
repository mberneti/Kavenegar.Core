using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kavenegar.Core.Dto.Result;
using Moq;
using NUnit.Framework;
using Shared.Infrastructure;
using Profile = Kavenegar.Core.KavenegarProfileApi;

namespace Test.Kavenegar.Core.KavenegarProfileApi;

[TestFixture]
public class Receive
{
    [SetUp]
    public void SetUp()
    {
        _mockHttpClientHelper = new Mock<IHttpClientHelper>();
        _kavenegarProfileApi = new Profile(_mockHttpClientHelper.Object, "");
    }

    private Profile _kavenegarProfileApi = null!;
    private Mock<IHttpClientHelper> _mockHttpClientHelper = null!;

    [Test]
    public async Task Receive_WhenCalled_CallsPostAsync()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/receive.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarProfileApi.Receive("line", false);

        _mockHttpClientHelper.Verify(
            i => i.PostAsync(
                "sms/receive.json",
                null,
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Receive_WhenCalled_CheckLineNumberValueInQueryParam()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/receive.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarProfileApi.Receive("linenumber", false);

        Assert.That(passedQueryParams["linenumber"], Is.EqualTo("linenumber"));
    }

    [Test]
    public async Task Receive_IsReadSetFalse_CheckIsReadValueIs0()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/receive.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarProfileApi.Receive("", false);

        Assert.That(passedQueryParams["isread"], Is.EqualTo(0));
    }

    [Test]
    public async Task Receive_IsReadSetTrue_CheckIsReadValueIs1()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/receive.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarProfileApi.Receive("", true);

        Assert.That(passedQueryParams["isread"], Is.EqualTo(1));
    }

    [Test]
    public async Task Receive_WhenCalled_ReturnsReceivedMessageDtoList()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/receive.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{\"entries\":[{}]}")
                });

        var result = await _kavenegarProfileApi.Receive("", true);

        Assert.That(result, Is.TypeOf<List<ReceivedMessageDto>>());
    }
}