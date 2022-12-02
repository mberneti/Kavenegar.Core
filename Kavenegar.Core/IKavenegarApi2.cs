using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kavenegar.Core.Models;
using Kavenegar.Core.Models.Enums;

namespace Kavenegar.Core;

public interface IKavenegarApi2
{
    Task<SendResult?> Send(
        SendSingleMessageRequest message,
        CancellationToken cancellationToken = default);

    Task<List<SendResult>> Send(
        SendMultiMessageRequest messages,
        CancellationToken cancellationToken = default);

    Task<StatusResult> Status(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<StatusResult>> Status(
        List<string> messageIds,
        CancellationToken cancellationToken = default);

    Task<StatusLocalMessageIdResult> StatusLocalMessageId(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<StatusLocalMessageIdResult>> StatusLocalMessageId(
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

    Task<CountOutboxResult> CountOutbox(
        DateTime startDate,
        DateTime? endDate,
        int? status,
        CancellationToken cancellationToken = default);

    Task<StatusResult> Cancel(
        string messageId,
        CancellationToken cancellationToken = default);

    Task<List<StatusResult>> Cancel(
        List<string> ids,
        CancellationToken cancellationToken = default);

    Task<List<ReceiveResult>> Receive(
        string line,
        bool isRead,
        CancellationToken cancellationToken = default);

    Task<CountInboxResult> CountInbox(
        DateTime startDate,
        DateTime? endDate,
        string? lineNumber,
        bool? isRead,
        CancellationToken cancellationToken = default);

    Task<AccountInfoResult> AccountInfo(
        CancellationToken cancellationToken = default);

    Task<AccountConfigResult> AccountConfig(
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