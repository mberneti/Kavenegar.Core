using Kavenegar.Core.Dto.Message;
using Kavenegar.Core.Dto.Result;
using Kavenegar.Core.Enums;

namespace Kavenegar.Core;

public interface IKavenegarMessageSender
{
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
    Task<SendResultDto?> Send(
        string message,
        string receptor,
        string sender = "",
        string localMessageId = "",
        DateTime? dateTime = null,
        bool hide = false,
        MessageType messageType = MessageType.AppMemory,
        CancellationToken cancellationToken = default);

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
    Task<List<SendResultDto>?> Send(
        string message,
        Dictionary<string, string?> receptors,
        string sender = "",
        DateTime? dateTime = null,
        bool hide = false,
        MessageType messageType = MessageType.AppMemory,
        CancellationToken cancellationToken = default);

    public Task<List<SendResultDto>?> Send(
        SendSingleMessageRequest message,
        CancellationToken cancellationToken = default);

    Task<List<SendResultDto>?> Send(
        IEnumerable<SendMessageInfo> sendMessageInfos,
        bool hide = false,
        DateTime? dateTime = null,
        CancellationToken cancellationToken = default);

    Task<List<SendResultDto>?> Send(
        SendMultiMessageRequest messages,
        CancellationToken cancellationToken = default);

    Task<SendResultDto?> VerifyLookup(
        string receptor,
        string template,
        string token1,
        string? token2 = null,
        string? token3 = null,
        string? token4 = null,
        string? token5 = null,
        VerifyLookupType? type = null,
        CancellationToken cancellationToken = default);
}