using System.Net;
using Kavenegar.Core.Dto.Message;
using Kavenegar.Core.Dto.Result;
using Kavenegar.Core.Enums;
using Shared.Infrastructure;

namespace Kavenegar.Core;

public class KavenegarMessageSender
    : BaseKavenegarApi,
        IKavenegarMessageSender
{
    public KavenegarMessageSender(
        IHttpClientHelper httpClientHelper,
        string apiKey) : base(httpClientHelper, apiKey)
    {
    }

    public KavenegarMessageSender(
        string apiKey) : base(new HttpClientHelper(new HttpClient()), apiKey)
    {
    }

    /// <summary>
    ///     Send one message for only the receptor
    /// </summary>
    /// <param name="message">Message text.</param>
    /// <param name="receptor">Who receives message.</param>
    /// <param name="sender">Number to send message if you set it empty the default number will be used.</param>
    /// <param name="localMessageId">Unique id which you set for the id.</param>
    /// <param name="dateTime">The date time you wand the message to be sent. If it is null message will be send asap.</param>
    /// <param name="hide">If true receptor number will be hidden.</param>
    /// <param name="messageType">Type of message.</param>
    /// <param name="cancellationToken">Token to cancel request.</param>
    /// <returns></returns>
    public async Task<SendResultDto?> Send(
        string message,
        string receptor,
        string sender = "",
        string localMessageId = "",
        DateTime? dateTime = null,
        bool hide = false,
        MessageType messageType = MessageType.Flash,
        CancellationToken cancellationToken = default)
    {
        var messageInfo = new MessageInfo(message)
        {
            Sender = sender,
            Type = messageType
        };

        var receptorLocalMessageIds = new Dictionary<string, string?>
        {
            {
                receptor, localMessageId
            }
        };

        var sendSingleMessageRequest = new SendSingleMessageRequest(messageInfo, receptorLocalMessageIds)
        {
            Hide = hide
        };

        if (dateTime.HasValue) sendSingleMessageRequest.Date = dateTime;

        return (await Send(sendSingleMessageRequest, cancellationToken))?.FirstOrDefault();
    }

    /// <summary>
    ///     Send message text for each receptor with the localMessageId
    /// </summary>
    /// <param name="message">Message text.</param>
    /// <param name="receptors">Key for receptor and value is for local message id.</param>
    /// <param name="sender">Number to send message if you set it empty the default number will be used.</param>
    /// <param name="dateTime">The date time you wand the message to be sent. If it is null message will be send asap.</param>
    /// <param name="hide">If true receptor number will be hidden.</param>
    /// <param name="messageType">Type of message.</param>
    /// <param name="cancellationToken">Token to cancel request.</param>
    /// <returns></returns>
    public async Task<List<SendResultDto>?> Send(
        string message,
        Dictionary<string, string?> receptors,
        string sender = "",
        DateTime? dateTime = null,
        bool hide = false,
        MessageType messageType = MessageType.Flash,
        CancellationToken cancellationToken = default)
    {
        var sendSingleMessageRequest = new SendSingleMessageRequest(
            new MessageInfo(message)
            {
                Sender = sender,
                Type = messageType
            },
            receptors)
        {
            Hide = hide
        };

        if (dateTime.HasValue) sendSingleMessageRequest.Date = dateTime;

        return await Send(sendSingleMessageRequest, cancellationToken);
    }

    public async Task<List<SendResultDto>?> Send(
        string message,
        IEnumerable<string> receptors,
        string sender = "",
        DateTime? dateTime = null,
        bool hide = false,
        MessageType messageType = MessageType.AppMemory,
        CancellationToken cancellationToken = default)
    {
        var sendSingleMessageRequest = new SendSingleMessageRequest(
            new MessageInfo(message)
            {
                Sender = sender,
                Type = messageType
            },
            receptors.ToDictionary(i => i, _ => "")!)
        {
            Hide = hide
        };

        if (dateTime.HasValue) sendSingleMessageRequest.Date = dateTime;

        return await Send(sendSingleMessageRequest, cancellationToken);
    }

    public async Task<List<SendResultDto>?> Send(
        SendSingleMessageRequest message,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "receptor", string.Join(',', message.ReceptorLocalMessageIds.Keys)
            },
            {
                "message", message.MessageInfo.Message
            },
            {
                "type", (int)message.MessageInfo.Type
            },
            {
                "date", message.Date?.ToUnixTimestamp() ?? 0
            }
        };

        if (message.MessageInfo.Sender.IsNotNullOrWhiteSpace()) queryParams.Add("sender", message.MessageInfo.Sender);

        if (message.ReceptorLocalMessageIds.Values.All(i => i.IsNotNullOrWhiteSpace()))
            queryParams.Add("localId", string.Join(',', message.ReceptorLocalMessageIds.Values));

        return await RequestSender<List<SendResultDto>>(
            "sms/send.json",
            null,
            queryParams,
            cancellationToken);
    }

    public async Task<List<SendResultDto>?> Send(
        IEnumerable<SendMessageInfo> sendMessageInfos,
        bool hide = false,
        DateTime? dateTime = null,
        CancellationToken cancellationToken = default)
    {
        return await Send(
            new SendMultiMessageRequest(sendMessageInfos)
            {
                Date = dateTime ?? DateTime.Now,
                Hide = hide
            },
            cancellationToken);
    }

    public async Task<List<SendResultDto>?> Send(
        SendMultiMessageRequest messages,
        CancellationToken cancellationToken = default)
    {
        var requestParams = new Dictionary<string, object?>
        {
            {
                "message",
                messages.SendMessageInfos
                    .Select(sendMessageInfo => WebUtility.HtmlEncode(sendMessageInfo.MessageInfo.Message))
                    .ToList()
                    .Serialize()
            },
            {
                "receptor",
                messages.SendMessageInfos.Select(sendMessageInfo => sendMessageInfo.Receptor).ToList().Serialize()
            },
            {
                "type",
                messages.SendMessageInfos.Select(sendMessageInfo => (int)sendMessageInfo.MessageInfo.Type).Serialize()
            },
            {
                "date", messages.Date == DateTime.MinValue ? 0 : messages.Date.ToUnixTimestamp()
            }
        };

        if (messages.SendMessageInfos.All(
                sendMessageInfo => sendMessageInfo.MessageInfo.Sender.IsNotNullOrWhiteSpace()))
            requestParams.Add(
                "sender",
                messages.SendMessageInfos.Select(sendMessageInfo => sendMessageInfo.MessageInfo.Sender)
                    .ToList()
                    .Serialize());

        if (messages.SendMessageInfos.All(
                sendMessageInfo => !string.IsNullOrWhiteSpace(sendMessageInfo.LocalMessageId)))
            requestParams.Add(
                "localMessageIds",
                string.Join(",", messages.SendMessageInfos.Select(sendMessageInfo => sendMessageInfo.LocalMessageId)));

        return await RequestSender<List<SendResultDto>>(
            "sms/sendarray.json",
            null,
            requestParams,
            cancellationToken);
    }

    public async Task<SendResultDto?> VerifyLookup(
        string receptor,
        string template,
        string token1,
        string? token2 = null,
        string? token3 = null,
        string? token4 = null,
        string? token5 = null,
        VerifyLookupType? type = null,
        CancellationToken cancellationToken = default)
    {
        return await VerifyLookup(
            new VerifyLookupRequest(
                receptor,
                template,
                token1)
            {
                Token2 = token2,
                Token3 = token3,
                Token4 = token4,
                Token5 = token5,
                VerifyLookupType = type
            },
            cancellationToken);
    }

    public async Task<SendResultDto?> VerifyLookup(
        VerifyLookupRequest verifyLookupRequest,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "receptor", verifyLookupRequest.Receptor
            },
            {
                "template", verifyLookupRequest.Template
            },
            {
                "token", verifyLookupRequest.Token1
            }
        };

        if (verifyLookupRequest.Token2.IsNotNullOrWhiteSpace()) queryParams.Add("token2", verifyLookupRequest.Token2);
        if (verifyLookupRequest.Token3.IsNotNullOrWhiteSpace()) queryParams.Add("token3", verifyLookupRequest.Token3);
        if (verifyLookupRequest.Token4.IsNotNullOrWhiteSpace()) queryParams.Add("token10", verifyLookupRequest.Token4);
        if (verifyLookupRequest.Token5.IsNotNullOrWhiteSpace()) queryParams.Add("token20", verifyLookupRequest.Token5);
        if (verifyLookupRequest.VerifyLookupType.HasValue)
            queryParams.Add("type", (int)verifyLookupRequest.VerifyLookupType.Value);

        return (await RequestSender<List<SendResultDto>>(
            "verify/lookup.json",
            null,
            queryParams,
            cancellationToken))?.FirstOrDefault();
    }
}