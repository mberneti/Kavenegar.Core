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
public class Cancel
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
    public async Task Cancel_SingleCalled_CallsPostAsync()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/cancel.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarProfileApi.Cancel("messageid");

        _mockHttpClientHelper.Verify(
            i => i.PostAsync(
                "sms/cancel.json",
                null,
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    [TestCase("1")]
    [TestCase("2")]
    public async Task Cancel_SingleCalled_CheckMessageIdsQueryParam(
        string messageId)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/cancel.json",
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

        await _kavenegarProfileApi.Cancel(messageId);

        Assert.That(passedQueryParams["messageid"], Is.EqualTo(messageId));
    }

    [Test]
    public async Task Cancel_SingleCalled_ReturnsStatusMessageDto()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/cancel.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{\"entries\":[{}]}")
                });

        var result = await _kavenegarProfileApi.Cancel("");

        Assert.That(result, Is.TypeOf<StatusMessageDto>());
    }

    [Test]
    public async Task Cancel_MultiCalled_CallsPostAsync()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/cancel.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarProfileApi.Cancel(
            new List<string>
            {
                ""
            });

        _mockHttpClientHelper.Verify(
            i => i.PostAsync(
                "sms/cancel.json",
                null,
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    [TestCase("1")]
    [TestCase("2")]
    public async Task Cancel_MultiCalled_CheckMessageIdsQueryParam(
        string messageId)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/cancel.json",
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

        await _kavenegarProfileApi.Cancel(
            new List<string>
            {
                messageId,
                messageId
            });

        Assert.That(passedQueryParams["messageid"], Is.EqualTo($"{messageId},{messageId}"));
    }

    [Test]
    public async Task Cancel_MultiCalled_ReturnsStatusMessageDto()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/cancel.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{\"entries\":[{}]}")
                });

        var result = await _kavenegarProfileApi.Cancel(
            new List<string>
            {
                ""
            });

        Assert.That(result, Is.TypeOf<List<StatusMessageDto>>());
    }
}