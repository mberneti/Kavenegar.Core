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
public class Status
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
    public async Task Status_SingleCalled_CallsPostAsync()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/status.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarProfileApi.Status("messageid");

        _mockHttpClientHelper.Verify(
            i => i.PostAsync(
                "sms/status.json",
                null,
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    [TestCase("1")]
    [TestCase("2")]
    public async Task Status_SingleCalled_CheckMessageIdsQueryParam(
        string messageId)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/status.json",
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

        await _kavenegarProfileApi.Status(messageId);

        Assert.That(passedQueryParams["messageid"], Is.EqualTo(messageId));
    }

    [Test]
    public async Task Status_SingleCalled_ReturnsStatusMessageDto()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/status.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{\"entries\":[{}]}")
                });

        var result = await _kavenegarProfileApi.Status("");

        Assert.That(result, Is.TypeOf<StatusMessageDto>());
    }

    [Test]
    public async Task Status_MultiCalled_CallsPostAsync()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/status.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarProfileApi.Status(
            new List<string>
            {
                ""
            });

        _mockHttpClientHelper.Verify(
            i => i.PostAsync(
                "sms/status.json",
                null,
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    [TestCase("1")]
    [TestCase("2")]
    public async Task Status_MultiCalled_CheckMessageIdsQueryParam(
        string messageId)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/status.json",
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

        await _kavenegarProfileApi.Status(
            new List<string>
            {
                messageId,
                messageId
            });

        Assert.That(passedQueryParams["messageid"], Is.EqualTo($"{messageId},{messageId}"));
    }

    [Test]
    public async Task Status_MultiCalled_ReturnsStatusMessageDto()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/status.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{\"entries\":[{}]}")
                });

        var result = await _kavenegarProfileApi.Status(
            new List<string>
            {
                ""
            });

        Assert.That(result, Is.TypeOf<List<StatusMessageDto>>());
    }
}