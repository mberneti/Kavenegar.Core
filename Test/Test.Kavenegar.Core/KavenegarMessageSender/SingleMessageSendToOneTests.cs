using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kavenegar.Core.Dto.Result;
using Kavenegar.Core.Enums;
using Moq;
using NUnit.Framework;
using Shared.Infrastructure;
using MessageSender = Kavenegar.Core.KavenegarMessageSender;

namespace Test.Kavenegar.Core.KavenegarMessageSender;

[TestFixture]
public class SingleMessageSendToOneTests
{
    [SetUp]
    public void SetUp()
    {
        _mockHttpClientHelper = new Mock<IHttpClientHelper>();
        _kavenegarMessageSender = new MessageSender(_mockHttpClientHelper.Object, "");
    }

    private MessageSender _kavenegarMessageSender = null!;
    private Mock<IHttpClientHelper> _mockHttpClientHelper = null!;

    [Test]
    public async Task Send_WhenCalled_CallsPostAsync()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}", Encoding.UTF8)
                });

        await _kavenegarMessageSender.Send("message", "receptor");

        _mockHttpClientHelper.Verify(
            i => i.PostAsync(
                "sms/send.json",
                null,
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Send_SenderIsEmpty_SenderNotInParams()
    {
        Dictionary<string, object?> passedQueryParams = null!;
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
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
                    Content = new StringContent("{}", Encoding.UTF8)
                });

        await _kavenegarMessageSender.Send("message", "receptor");

        Assert.That(passedQueryParams.ContainsKey("sender"), Is.False);
    }

    [Test]
    public async Task Send_SenderNotEmpty_SenderInParams()
    {
        Dictionary<string, object?> passedQueryParams = null!;
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
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
                    Content = new StringContent("{}", Encoding.UTF8)
                });

        await _kavenegarMessageSender.Send(
            "message",
            "receptor",
            "sender");

        Assert.That(passedQueryParams["sender"], Is.EqualTo("sender"));
    }

    [Test]
    public async Task Send_LocalMessageIdsAreNotQualified_LocalIdNotInParams()
    {
        Dictionary<string, object?> passedQueryParams = null!;
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
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
                    Content = new StringContent("{}", Encoding.UTF8)
                });

        await _kavenegarMessageSender.Send("message", "receptor");

        Assert.That(passedQueryParams.ContainsKey("localId"), Is.False);
    }

    [Test]
    public async Task Send_LocalMessageIdsAreQualified_LocalIdInParams()
    {
        Dictionary<string, object?> passedQueryParams = null!;
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
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
                    Content = new StringContent("{}", Encoding.UTF8)
                });

        await _kavenegarMessageSender.Send(
            "message",
            "receptor",
            localMessageId: "localMessageId");

        Assert.That(passedQueryParams.ContainsKey("localId"), Is.True);
    }

    [Test]
    public async Task Send_WhenCalled_CheckStaticParams()
    {
        Dictionary<string, object?> passedQueryParams = null!;
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
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
                    Content = new StringContent("{}", Encoding.UTF8)
                });

        await _kavenegarMessageSender.Send("message", "receptor");

        Assert.That(passedQueryParams["receptor"], Is.EqualTo("receptor"));
        Assert.That(passedQueryParams["message"], Is.EqualTo("message"));
    }

    [Test]
    public async Task Send_DefaultDate_DateParamIsZero()
    {
        Dictionary<string, object?> passedQueryParams = null!;
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
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
                    Content = new StringContent("{}", Encoding.UTF8)
                });

        await _kavenegarMessageSender.Send("message", "receptor");

        Assert.That(passedQueryParams["date"], Is.EqualTo(0));
    }

    [Test]
    public async Task Send_SpecificDate_DateParamHasValue()
    {
        Dictionary<string, object?> passedQueryParams = null!;
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
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
                    Content = new StringContent("{}", Encoding.UTF8)
                });

        var dt = DateTime.Now;
        await _kavenegarMessageSender.Send(
            "message",
            "receptor",
            dateTime: dt);

        Assert.That(passedQueryParams["date"], Is.EqualTo(dt.ToUnixTimestamp()));
    }

    [Test]
    public async Task Send_TypeNotSet_TypeValueIsDefault()
    {
        Dictionary<string, object?> passedQueryParams = null!;
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
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
                    Content = new StringContent("{}", Encoding.UTF8)
                });

        await _kavenegarMessageSender.Send("message", "receptor");

        Assert.That(passedQueryParams["type"], Is.EqualTo((int)MessageType.Flash));
    }

    [Test]
    [TestCase(MessageType.AppMemory)]
    [TestCase(MessageType.MobileMemory)]
    [TestCase(MessageType.SimMemory)]
    public async Task Send_TypeSet_CheckTypeValue(
        MessageType messageType)
    {
        Dictionary<string, object?> passedQueryParams = null!;
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
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
                    Content = new StringContent("{}", Encoding.UTF8)
                });

        await _kavenegarMessageSender.Send(
            "message",
            "receptor",
            messageType: messageType);

        Assert.That(passedQueryParams["type"], Is.EqualTo((int)messageType));
    }

    [Test]
    public async Task Send_PostRequestReturnsNoResult_ResultIsNull()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}", Encoding.UTF8)
                });

        var result = await _kavenegarMessageSender.Send("message", "receptor");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Send_PostRequestReturnsResult_ResultSendResultDto()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/send.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{\"entries\":[{}]}", Encoding.UTF8)
                });

        var result = await _kavenegarMessageSender.Send("message", "receptor");

        Assert.That(result, Is.TypeOf<SendResultDto>());
    }
}