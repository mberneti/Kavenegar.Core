using Kavenegar.Core.Dto.Result;
using Kavenegar.Core.Enums;

namespace Kavenegar.Core;

public interface IKavenegarProfileApi
{
    Task<StatusMessageDto?> Status(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<StatusMessageDto>?> Status(
        IEnumerable<string> messageIds,
        CancellationToken cancellationToken = default);

    Task<LocalStatusDto?> StatusLocalMessageId(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<LocalStatusDto>?> StatusLocalMessageId(
        IEnumerable<string> messageIds,
        CancellationToken cancellationToken = default);

    Task<SendResultDto?> Select(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<SendResultDto>?> Select(
        IEnumerable<string> messageIds,
        CancellationToken cancellationToken = default);

    Task<List<SendResultDto>?> SelectOutbox(
        DateTime startDate,
        DateTime? endDate = null,
        string? sender = null,
        CancellationToken cancellationToken = default);

    Task<List<SendResultDto>?> LatestOutbox(
        int? pageSize = null,
        string? sender = null,
        CancellationToken cancellationToken = default);

    Task<CountOutboxDto?> CountOutbox(
        DateTime startDate,
        DateTime? endDate = null,
        MessageStatus? status = null,
        CancellationToken cancellationToken = default);

    Task<StatusMessageDto?> Cancel(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<StatusMessageDto>?> Cancel(
        IEnumerable<string> ids,
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
        string? apiLogs = null,
        string? dailyReport = null,
        string? debugMode = null,
        string? defaultSender = null,
        int? minCreditAlarm = null,
        string? resendFailed = null,
        CancellationToken cancellationToken = default);
}