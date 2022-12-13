using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kavenegar.Core.Dto.Result;
using Kavenegar.Core.Enums;
using Moq;
using NUnit.Framework;
using Shared.Infrastructure;
using Profile = Kavenegar.Core.KavenegarProfileApi;

namespace Test.Kavenegar.Core.KavenegarProfileApi;

public class CountOutbox
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
    public async Task CountOutbox_WhenCalled_CallsPostAsync()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countoutbox.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarProfileApi.CountOutbox(DateTime.Now);

        _mockHttpClientHelper.Verify(
            i => i.PostAsync(
                "sms/countoutbox.json",
                null,
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    [TestCase(61)]
    [TestCase(71)]
    [TestCase(81)]
    public void CountOutbox_StartDate61DaysAgo_ThrowsArgumentException(
        int daysAgo)
    {
        Assert.That(
            async () => await _kavenegarProfileApi.CountOutbox(DateTime.Now.AddDays(-1 * daysAgo)),
            Throws.ArgumentException);
    }

    [Test]
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(100)]
    public void CountOutbox_StartDateGreaterThanOrEqualEndDate_ThrowsArgumentException(
        int secondsBeforeStartDate)
    {
        Assert.That(
            async () => await _kavenegarProfileApi.CountOutbox(
                DateTime.Now,
                DateTime.Now.AddSeconds(-1 * secondsBeforeStartDate)),
            Throws.ArgumentException);
    }

    [Test]
    [TestCase(2)]
    [TestCase(10)]
    [TestCase(100)]
    public void CountOutbox_StartDateEndDateDifferenceMoreThan1Day_ThrowsArgumentException(
        int daysBeforeStartDate)
    {
        Assert.That(
            async () => await _kavenegarProfileApi.CountOutbox(DateTime.Now, DateTime.Now.AddDays(daysBeforeStartDate)),
            Throws.ArgumentException);
    }

    [Test]
    public async Task CountOutbox_WhenCalled_CheckStartDateQueryParam()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countoutbox.json",
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

        await _kavenegarProfileApi.CountOutbox(expectedStartDateTime);

        Assert.That(passedQueryParams["startdate"], Is.EqualTo(expectedStartDateTime.ToUnixTimestamp()));
    }

    [Test]
    public async Task CountOutbox_WithNoEndDate_QueryParamsNotIncludeEndDate()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countoutbox.json",
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

        await _kavenegarProfileApi.CountOutbox(expectedStartDateTime);

        Assert.That(passedQueryParams.ContainsKey("enddate"), Is.False);
    }

    [Test]
    public async Task CountOutbox_WithEndDate_QueryParamsIncludeEndDate()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countoutbox.json",
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

        await _kavenegarProfileApi.CountOutbox(expectedStartDateTime, expectedEndDateDateTime);

        Assert.That(passedQueryParams["enddate"], Is.EqualTo(expectedEndDateDateTime.ToUnixTimestamp()));
    }

    [Test]
    public async Task CountOutbox_WithNoStatus_QueryParamsNotIncludeStatus()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countoutbox.json",
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

        await _kavenegarProfileApi.CountOutbox(expectedStartDateTime);

        Assert.That(passedQueryParams.ContainsKey("status"), Is.False);
    }

    [Test]
    public async Task CountOutbox_WithStatus_QueryParamsIncludeStatus()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countoutbox.json",
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

        await _kavenegarProfileApi.CountOutbox(expectedStartDateTime, status: MessageStatus.Canceled);

        Assert.That(passedQueryParams["status"], Is.EqualTo((int)MessageStatus.Canceled));
    }

    [Test]
    public async Task CountOutbox_WhenCalled_ReturnsSendResultDtoList()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/countoutbox.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{\"entries\":[{}]}")
                });

        var result = await _kavenegarProfileApi.CountOutbox(DateTime.Now);

        Assert.That(result, Is.TypeOf<CountOutboxDto>());
    }
}