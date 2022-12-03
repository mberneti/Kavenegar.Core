using Kavenegar.Core.Dto.Message;
using Kavenegar.Core.Dto.Result;
using Kavenegar.Core.Enums;

namespace Kavenegar.Core;

public interface IKavenegarApi
{
    Task<SendResult?> Send(
        SendSingleMessageRequest message,
        CancellationToken cancellationToken = default);

    Task<List<SendResult>> Send(
        SendMultiMessageRequest messages,
        CancellationToken cancellationToken = default);

    Task<StatusMessageDto> Status(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<StatusMessageDto>> Status(
        List<string> messageIds,
        CancellationToken cancellationToken = default);

    Task<LocalStatusDto> StatusLocalMessageId(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<LocalStatusDto>> StatusLocalMessageId(
        List<string> messageIds,
        CancellationToken cancellationToken = default);

    Task<SendResult> Select(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<SendResult>> Select(
        List<string> messageIds,
        CancellationToken cancellationToken = default);

    Task<List<SendResult>> SelectOutbox(
        DateTime startDate,
        DateTime? endDate,
        string? sender,
        CancellationToken cancellationToken = default);

    Task<List<SendResult>> LatestOutbox(
        long? pageSize,
        string? sender,
        CancellationToken cancellationToken = default);

    Task<CountOutboxDto> CountOutbox(
        DateTime startDate,
        DateTime? endDate,
        int? status,
        CancellationToken cancellationToken = default);

    Task<StatusMessageDto> Cancel(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<StatusMessageDto>> Cancel(
        List<string> ids,
        CancellationToken cancellationToken = default);

    Task<List<ReceivedMessageDto>> Receive(
        string line,
        bool isRead,
        CancellationToken cancellationToken = default);

    Task<CountInboxDto> CountInbox(
        DateTime startDate,
        DateTime? endDate,
        string? lineNumber,
        bool? isRead,
        CancellationToken cancellationToken = default);

    Task<AccountInfoDto> AccountInfo(
        CancellationToken cancellationToken = default);

    Task<AccountConfigDto> AccountConfig(
        string apiLogs,
        string dailyReport,
        string debugMode,
        string defaultSender,
        int? minCreditAlarm,
        string resendFailed,
        CancellationToken cancellationToken = default);

    Task<SendResult> VerifyLookup(
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