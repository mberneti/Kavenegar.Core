using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kavenegar.Core.Dto.Message;
using Kavenegar.Core.Enums;
using Moq;
using NUnit.Framework;
using Shared.Infrastructure;
using MessageSender = Kavenegar.Core.KavenegarMessageSender;

namespace Test.Kavenegar.Core.KavenegarMessageSender.MultiMessage;

[TestFixture]
public class SendWithDtoTests
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
                    "sms/sendarray.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.Send(new SendMultiMessageRequest(new List<SendMessageInfo>()));

        _mockHttpClientHelper.Verify(
            i => i.PostAsync(
                "sms/sendarray.json",
                null,
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    [TestCase("test")]
    [TestCase("تست")]
    public async Task Send_WhenCalled_CheckMessageQueryParam(
        string message)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/sendarray.json",
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

        await _kavenegarMessageSender.Send(
            new SendMultiMessageRequest(
                new List<SendMessageInfo>
                {
                    new(new MessageInfo(message), ""),
                    new(new MessageInfo(message), "")
                }));

        var expectedMessages = new List<string>
        {
            WebUtility.HtmlEncode(message),
            WebUtility.HtmlEncode(message)
        }.Serialize();

        Assert.That(passedQueryParams["message"], Is.EqualTo(expectedMessages));
    }

    [Test]
    public async Task Send_WithNoSender_QueryParamsNotIncludeSender()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/sendarray.json",
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

        await _kavenegarMessageSender.Send(
            new SendMultiMessageRequest(
                new List<SendMessageInfo>
                {
                    new(new MessageInfo(""), "")
                }));

        Assert.That(passedQueryParams.ContainsKey("sender"), Is.False);
    }

    [Test]
    [TestCase("1")]
    [TestCase("2")]
    public async Task Send_WithSender_QueryParamsIncludeSender(
        string sender)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/sendarray.json",
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

        await _kavenegarMessageSender.Send(
            new SendMultiMessageRequest(
                new List<SendMessageInfo>
                {
                    new(new MessageInfo(""), "")
                    {
                        MessageInfo = new MessageInfo("")
                        {
                            Sender = sender
                        }
                    },
                    new(new MessageInfo(""), "")
                    {
                        MessageInfo = new MessageInfo("")
                        {
                            Sender = sender
                        }
                    }
                }));

        Assert.That(passedQueryParams["sender"], Is.EqualTo($"[\"{sender}\",\"{sender}\"]"));
    }

    [Test]
    [TestCase("1")]
    [TestCase("2")]
    public async Task Send_WhenCalled_CheckReceptorQueryParam(string receptor)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/sendarray.json",
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

        await _kavenegarMessageSender.Send(
            new SendMultiMessageRequest(
                new List<SendMessageInfo>
                {
                    new(new MessageInfo(""), receptor)
                    {
                        MessageInfo = new MessageInfo("")
                    },
                    new(new MessageInfo(""), receptor)
                    {
                        MessageInfo = new MessageInfo("")
                    }
                }));

        Assert.That(passedQueryParams["receptor"], Is.EqualTo($"[\"{receptor}\",\"{receptor}\"]"));
    }

    [Test]
    [TestCase(MessageType.Flash)]
    [TestCase(MessageType.AppMemory)]
    public async Task Send_WhenCalled_CheckTypeQueryParam(MessageType messageType)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/sendarray.json",
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

        await _kavenegarMessageSender.Send(
            new SendMultiMessageRequest(
                new List<SendMessageInfo>
                {
                    new(new MessageInfo(""), "")
                    {
                        MessageInfo = new MessageInfo("")
                        {
                            Type = messageType
                        }
                    },
                    new(new MessageInfo(""), "")
                    {
                        MessageInfo = new MessageInfo("")
                        {
                            Type = messageType
                        }
                    }
                }));

        Assert.That(passedQueryParams["type"], Is.EqualTo($"[{(int)messageType},{(int)messageType}]"));
    }

    [Test]
    public async Task Send_WhenCalled_CheckDateQueryParam()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/sendarray.json",
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

        var dt = DateTime.Now;

        await _kavenegarMessageSender.Send(
            new SendMultiMessageRequest(
                new List<SendMessageInfo>
                {
                    new(new MessageInfo(""), "")
                    {
                        MessageInfo = new MessageInfo("")
                    },
                    new(new MessageInfo(""), "")
                    {
                        MessageInfo = new MessageInfo("")
                    }
                })
            {
                Date = dt
            });

        Assert.That(passedQueryParams["date"], Is.EqualTo(dt.ToUnixTimestamp()));
    }

    [Test]
    [TestCase("1")]
    [TestCase("2")]
    public async Task Send_WithLocalMessageId_QueryParamsIncludeLocalMessageId(string localMessageId)
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "sms/sendarray.json",
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

        await _kavenegarMessageSender.Send(
            new SendMultiMessageRequest(
                new List<SendMessageInfo>
                {
                    new(new MessageInfo(""), "")
                    {
                        MessageInfo = new MessageInfo(""),
                        LocalMessageId = localMessageId
                    },
                    new(new MessageInfo(""), "")
                    {
                        MessageInfo = new MessageInfo(""),
                        LocalMessageId = localMessageId
                    }
                }));

        Assert.That(passedQueryParams["localMessageIds"], Is.EqualTo($"{localMessageId},{localMessageId}"));
    }
}