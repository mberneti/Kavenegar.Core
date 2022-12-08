using Kavenegar.Core.Dto.Result;

namespace Kavenegar.Core;

public interface IKavenegarProfileApi
{
    Task<StatusMessageDto?> Status(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<StatusMessageDto>?> Status(
        List<string> messageIds,
        CancellationToken cancellationToken = default);

    Task<LocalStatusDto?> StatusLocalMessageId(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<LocalStatusDto>?> StatusLocalMessageId(
        List<string> messageIds,
        CancellationToken cancellationToken = default);

    Task<SendResultDto?> Select(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<SendResultDto>?> Select(
        List<string> messageIds,
        CancellationToken cancellationToken = default);

    Task<List<SendResultDto>?> SelectOutbox(
        DateTime startDate,
        DateTime? endDate,
        string? sender,
        CancellationToken cancellationToken = default);

    Task<List<SendResultDto>?> LatestOutbox(
        long? pageSize,
        string? sender,
        CancellationToken cancellationToken = default);

    Task<CountOutboxDto?> CountOutbox(
        DateTime startDate,
        DateTime? endDate,
        int? status,
        CancellationToken cancellationToken = default);

    Task<StatusMessageDto?> Cancel(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<StatusMessageDto>?> Cancel(
        List<string> ids,
        CancellationToken cancellationToken = default);

    Task<List<ReceivedMessageDto>?> Receive(
        string line,
        bool isRead,
        CancellationToken cancellationToken = default);

    Task<CountInboxDto?> CountInbox(
        DateTime startDate,
        DateTime? endDate,
        string? lineNumber,
        bool? isRead,
        CancellationToken cancellationToken = default);

    Task<AccountInfoDto?> AccountInfo(
        CancellationToken cancellationToken = default);

    Task<AccountConfigDto?> AccountConfig(
        string apiLogs,
        string dailyReport,
        string debugMode,
        string defaultSender,
        int? minCreditAlarm,
        string resendFailed,
        CancellationToken cancellationToken = default);
}