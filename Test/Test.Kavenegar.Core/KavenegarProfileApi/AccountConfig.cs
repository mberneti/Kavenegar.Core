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

public class AccountConfig
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
    public async Task AccountConfig_WithAtLeastOneArgumentPassed_ReturnsAccountConfigDto()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{\"entries\":{}}")
                });

        var result = await _kavenegarProfileApi.AccountConfig(dailyReport: "a");

        Assert.That(result, Is.TypeOf<AccountConfigDto>());
    }

    [Test]
    public void AccountConfig_NoSettingArgumentIsSet_ThrowsArgumentException()
    {
        Assert.That(async () => await _kavenegarProfileApi.AccountConfig(), Throws.ArgumentException);
    }

    [Test]
    public async Task AccountConfig_WithNoApiLogs_QueryParamsNotIncludeApiLogs()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig(dailyReport: "a");

        Assert.That(passedQueryParams.ContainsKey("apilogs"), Is.False);
    }

    [Test]
    public async Task AccountConfig_WithApiLogs_QueryParamsIncludeApiLogs()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig("apilogs");

        Assert.That(passedQueryParams["apilogs"], Is.EqualTo("apilogs"));
    }

    [Test]
    public async Task AccountConfig_WithNoDailyReport_QueryParamsNotIncludeDailyReport()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig("a");

        Assert.That(passedQueryParams.ContainsKey("dailyreport"), Is.False);
    }

    [Test]
    public async Task AccountConfig_WithDailyReport_QueryParamsIncludeDailyReport()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig(dailyReport: "dailyreport");

        Assert.That(passedQueryParams["dailyreport"], Is.EqualTo("dailyreport"));
    }

    [Test]
    public async Task AccountConfig_WithNoDebugMode_QueryParamsNotIncludeDebugMode()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig("a");

        Assert.That(passedQueryParams.ContainsKey("debugmode"), Is.False);
    }

    [Test]
    public async Task AccountConfig_WithDebugMode_QueryParamsIncludeDebugMode()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig(debugMode: "debugmode");

        Assert.That(passedQueryParams["debugmode"], Is.EqualTo("debugmode"));
    }

    [Test]
    public async Task AccountConfig_WithNoDefaultSender_QueryParamsNotIncludeDefaultSender()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig("a");

        Assert.That(passedQueryParams.ContainsKey("defaultsender"), Is.False);
    }

    [Test]
    public async Task AccountConfig_WithDefaultSender_QueryParamsIncludeDefaultSender()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig(defaultSender: "defaultsender");

        Assert.That(passedQueryParams["defaultsender"], Is.EqualTo("defaultsender"));
    }

    [Test]
    public async Task AccountConfig_WithNoMinCreditAlarm_QueryParamsNotIncludeMinCreditAlarm()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig("a");

        Assert.That(passedQueryParams.ContainsKey("mincreditalarm"), Is.False);
    }

    [Test]
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(100)]
    public async Task AccountConfig_WithMinCreditAlarm_QueryParamsIncludeMinCreditAlarm(
        int mincreditalarm)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig(minCreditAlarm: mincreditalarm);

        Assert.That(passedQueryParams["mincreditalarm"], Is.EqualTo(mincreditalarm));
    }

    [Test]
    public async Task AccountConfig_WithNoResendFailed_QueryParamsNotIncludeResendFailed()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig("a");

        Assert.That(passedQueryParams.ContainsKey("resendfailed"), Is.False);
    }

    [Test]
    public async Task AccountConfig_WithResendFailed_QueryParamsIncludeResendFailed()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "account/config.json",
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

        await _kavenegarProfileApi.AccountConfig(resendFailed: "resendfailed");

        Assert.That(passedQueryParams["resendfailed"], Is.EqualTo("resendfailed"));
    }
}