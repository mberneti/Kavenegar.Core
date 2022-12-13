using System;
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

public class CountInbox
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
    public async Task CountInbox_WhenCalled_CallsPostAsync()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countinbox.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarProfileApi.CountInbox(DateTime.Now);

        _mockHttpClientHelper.Verify(
            i => i.PostAsync(
                "sms/countinbox.json",
                null,
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    [TestCase(61)]
    [TestCase(71)]
    [TestCase(81)]
    public void CountInbox_StartDate61DaysAgo_ThrowsArgumentException(
        int daysAgo)
    {
        Assert.That(
            async () => await _kavenegarProfileApi.CountInbox(DateTime.Now.AddDays(-1 * daysAgo)),
            Throws.ArgumentException);
    }

    [Test]
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(100)]
    public void CountInbox_StartDateGreaterThanOrEqualEndDate_ThrowsArgumentException(
        int secondsBeforeStartDate)
    {
        Assert.That(
            async () => await _kavenegarProfileApi.CountInbox(
                DateTime.Now,
                DateTime.Now.AddSeconds(-1 * secondsBeforeStartDate)),
            Throws.ArgumentException);
    }

    [Test]
    [TestCase(2)]
    [TestCase(10)]
    [TestCase(100)]
    public void CountInbox_StartDateEndDateDifferenceMoreThan1Day_ThrowsArgumentException(
        int daysBeforeStartDate)
    {
        Assert.That(
            async () => await _kavenegarProfileApi.CountInbox(DateTime.Now, DateTime.Now.AddDays(daysBeforeStartDate)),
            Throws.ArgumentException);
    }

    [Test]
    public async Task CountInbox_WhenCalled_CheckStartDateQueryParam()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countinbox.json",
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

        var expectedStartDateTime = DateTime.Now;

        await _kavenegarProfileApi.CountInbox(expectedStartDateTime);

        Assert.That(passedQueryParams["startdate"], Is.EqualTo(expectedStartDateTime.ToUnixTimestamp()));
    }

    [Test]
    public async Task CountInbox_WithNoEndDate_QueryParamsNotIncludeEndDate()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countinbox.json",
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

        var expectedStartDateTime = DateTime.Now;

        await _kavenegarProfileApi.CountInbox(expectedStartDateTime);

        Assert.That(passedQueryParams.ContainsKey("enddate"), Is.False);
    }

    [Test]
    public async Task CountInbox_WithEndDate_QueryParamsIncludeEndDate()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countinbox.json",
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

        var expectedStartDateTime = DateTime.Now;
        var expectedEndDateDateTime = expectedStartDateTime.AddHours(1);

        await _kavenegarProfileApi.CountInbox(expectedStartDateTime, expectedEndDateDateTime);

        Assert.That(passedQueryParams["enddate"], Is.EqualTo(expectedEndDateDateTime.ToUnixTimestamp()));
    }

    [Test]
    public async Task CountInbox_WithNoLineNumber_QueryParamsNotIncludeLineNumber()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countinbox.json",
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

        var expectedStartDateTime = DateTime.Now;

        await _kavenegarProfileApi.CountInbox(expectedStartDateTime);

        Assert.That(passedQueryParams.ContainsKey("linenumber"), Is.False);
    }

    [Test]
    public async Task CountInbox_WithLineNumber_QueryParamsIncludeLineNumber()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countinbox.json",
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

        await _kavenegarProfileApi.CountInbox(DateTime.Now, lineNumber: "linenumber");

        Assert.That(passedQueryParams["linenumber"], Is.EqualTo("linenumber"));
    }

    [Test]
    public async Task CountInbox_WithNoIsRead_QueryParamsNotIncludeIsRead()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countinbox.json",
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

        var expectedStartDateTime = DateTime.Now;

        await _kavenegarProfileApi.CountInbox(expectedStartDateTime);

        Assert.That(passedQueryParams.ContainsKey("isread"), Is.False);
    }

    [Test]
    public async Task CountInbox_WithIsReadSetTrue_QueryParamsIsReadEqual1()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countinbox.json",
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

        await _kavenegarProfileApi.CountInbox(DateTime.Now, isRead: true);

        Assert.That(passedQueryParams["isread"], Is.EqualTo(1));
    }

    [Test]
    public async Task CountInbox_WithIsReadSetFalse_QueryParamsIsReadEqual0()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countinbox.json",
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

        await _kavenegarProfileApi.CountInbox(DateTime.Now, isRead: false);

        Assert.That(passedQueryParams["isread"], Is.EqualTo(0));
    }

    [Test]
    public async Task CountInbox_WhenCalled_ReturnsSendResultDtoList()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countinbox.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{\"entries\":[{}]}")
                });

        var result = await _kavenegarProfileApi.CountInbox(DateTime.Now);

        Assert.That(result, Is.TypeOf<CountInboxDto>());
    }
}