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

public class LatestOutbox
{
    private Profile _kavenegarProfileApi = null!;
    private Mock<IHttpClientHelper> _mockHttpClientHelper = null!;

    [SetUp]
    public void SetUp()
    {
        _mockHttpClientHelper = new Mock<IHttpClientHelper>();
        _kavenegarProfileApi = new Profile(_mockHttpClientHelper.Object, "");
    }

    [Test]
    [TestCase(501)]
    [TestCase(601)]
    [TestCase(701)]
    public void LatestOutbox_PageSizeLargerThan500_ThrowsArgumentException(
        int pageSize)
    {
        Assert.That(async () => await _kavenegarProfileApi.LatestOutbox(501), Throws.ArgumentException);
    }

    [Test]
    public async Task LatestOutbox_NullPageSize_PageSizeIs500()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/latestoutbox.json",
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

        await _kavenegarProfileApi.LatestOutbox();

        Assert.That(passedQueryParams["pagesize"], Is.EqualTo(500));
    }

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(100)]
    public async Task LatestOutbox_SetPageSize_CheckPageSizeQueryParam(
        int pageSize)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/latestoutbox.json",
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

        await _kavenegarProfileApi.LatestOutbox(pageSize);

        Assert.That(passedQueryParams["pagesize"], Is.EqualTo(pageSize));
    }

    [Test]
    public async Task LatestOutbox_WithNoSender_QueryParamsNotIncludeSender()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/latestoutbox.json",
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

        await _kavenegarProfileApi.LatestOutbox();

        Assert.That(passedQueryParams.ContainsKey("sender"), Is.False);
    }

    [Test]
    [TestCase("a")]
    [TestCase("b")]
    [TestCase("c")]
    public async Task LatestOutbox_WithSender_QueryParamsIncludeSender(
        string sender)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/latestoutbox.json",
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

        await _kavenegarProfileApi.LatestOutbox(sender: sender);

        Assert.That(passedQueryParams["sender"], Is.EqualTo(sender));
    }

    [Test]
    public async Task LatestOutbox_WhenCalled_ReturnsSendResultDtoList()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/latestoutbox.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{\"entries\":[{}]}")
                });

        var result = await _kavenegarProfileApi.LatestOutbox();

        Assert.That(result, Is.TypeOf<List<SendResultDto>>());
    }
}