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
        MessageType messageType = MessageType.AppMemory,
        CancellationToken cancellationToken = default)
    {
        return (await Send(
            new SendSingleMessageRequest
            {
                Date = dateTime ?? DateTime.Now,
                Hide = hide,
                MessageInfo = new MessageInfo
                {
                    Message = message,
                    Sender = sender,
                    Type = messageType
                },
                ReceptorLocalMessageIds = new Dictionary<string, string>
                {
                    {
                        receptor, localMessageId
                    }
                }
            },
            cancellationToken))?.FirstOrDefault();
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
        Dictionary<string, string> receptors,
        string sender = "",
        DateTime? dateTime = null,
        bool hide = false,
        MessageType messageType = MessageType.AppMemory,
        CancellationToken cancellationToken = default)
    {
        return await Send(
            new SendSingleMessageRequest
            {
                Date = dateTime ?? DateTime.Now,
                Hide = hide,
                MessageInfo = new MessageInfo
                {
                    Message = message,
                    Sender = sender,
                    Type = messageType
                },
                ReceptorLocalMessageIds = receptors
            },
            cancellationToken);
    }

    public async Task<List<SendResultDto>?> Send(
        SendSingleMessageRequest message,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "sender", message.MessageInfo.Sender
            },
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

        if (message.ReceptorLocalMessageIds.Values.All(string.IsNullOrWhiteSpace))
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
            new SendMultiMessageRequest
            {
                Date = dateTime ?? DateTime.Now,
                Hide = hide,
                SendMessageInfos = sendMessageInfos
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
                await messages.SendMessageInfos
                    .Select(sendMessageInfo => WebUtility.HtmlEncode(sendMessageInfo.MessageInfo.Message))
                    .ToList()
                    .Serialize(cancellationToken)
            },
            {
                "sender",
                await messages.SendMessageInfos.Select(sendMessageInfo => sendMessageInfo.MessageInfo.Sender)
                    .ToList()
                    .Serialize(cancellationToken)
            },
            {
                "receptor",
                await messages.SendMessageInfos.Select(sendMessageInfo => sendMessageInfo.Receptor)
                    .ToList()
                    .Serialize(cancellationToken)
            },
            {
                "type",
                await messages.SendMessageInfos.Select(sendMessageInfo => sendMessageInfo.MessageInfo.Type.ToString())
                    .Serialize(cancellationToken)
            },
            {
                "date", messages.Date == DateTime.MinValue ? 0 : messages.Date.ToUnixTimestamp()
            }
        };

        if (messages.SendMessageInfos.All(
                sendMessageInfo => !string.IsNullOrWhiteSpace(sendMessageInfo.LocalMessageId)))
            requestParams.Add(
                "localMessageIds",
                string.Join(
                    ",",
                    messages.SendMessageInfos.Select(
                        sendMessageInfo => !string.IsNullOrWhiteSpace(sendMessageInfo.LocalMessageId))));

        return await RequestSender<List<SendResultDto>>(
            "sms/sendarray.json",
            null,
            requestParams,
            cancellationToken);
    }

    public async Task<SendResultDto?> VerifyLookup(
        string? receptor,
        string? template,
        string? token1,
        string? token2 = null,
        string? token3 = null,
        string? token4 = null,
        string? token5 = null,
        VerifyLookupType? type = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "receptor", receptor
            },
            {
                "template", template
            },
            {
                "token", token1
            }
        };

        if (string.IsNullOrWhiteSpace(token2)) queryParams.Add("token2", token2);
        if (string.IsNullOrWhiteSpace(token3)) queryParams.Add("token3", token3);
        if (string.IsNullOrWhiteSpace(token4)) queryParams.Add("token10", token4);
        if (string.IsNullOrWhiteSpace(token5)) queryParams.Add("token20", token5);
        if (type.HasValue) queryParams.Add("type", type.Value.ToString());

        return (await RequestSender<List<SendResultDto>>(
            "verify/lookup.json",
            null,
            queryParams,
            cancellationToken))?.FirstOrDefault();
    }
}