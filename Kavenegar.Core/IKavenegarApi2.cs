using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kavenegar.Core.Models;
using Kavenegar.Core.Models.Enums;

namespace Kavenegar.Core;

public interface IKavenegarApi2
{
    string ApiKey { set; get; }

    Task<List<SendResult>> Send(
        SendMessageRequest message,
        DateTime? date);

    Task<List<SendResult>> SendArray(
        List<SendMessageRequest> messages,
        DateTime? date);

    Task<StatusResult> Status(
        string messageId);

    Task<List<StatusResult>> Status(
        List<string> messageIds);

    Task<StatusLocalMessageIdResult> StatusLocalMessageId(
        string messageId);

    Task<List<StatusLocalMessageIdResult>> StatusLocalMessageId(
        List<string> messageIds);

    Task<SendResult> Select(
        string messageId);

    Task<List<SendResult>> Select(
        List<string> messageIds);

    Task<List<SendResult>> SelectOutbox(
        DateTime startDate,
        DateTime? endDate,
        string? sender);

    Task<List<SendResult>> LatestOutbox(
        long? pageSize,
        string? sender);

    Task<CountOutboxResult> CountOutbox(
        DateTime startDate,
        DateTime? endDate,
        int? status);

    Task<StatusResult> Cancel(
        string messageId);

    Task<List<StatusResult>> Cancel(
        List<string> ids);

    Task<List<ReceiveResult>> Receive(
        string line,
        int isRead);

    Task<CountInboxResult> CountInbox(
        DateTime startDate,
        DateTime? endDate,
        string? lineNumber,
        int? isRead);

    Task<AccountInfoResult> AccountInfo();

    Task<AccountConfigResult> AccountConfig(
        string apiLogs,
        string dailyReport,
        string debugMode,
        string defaultSender,
        int? minCreditAlarm,
        string resendFailed);

    Task<SendResult> VerifyLookup(
        string receptor,
        string template,
        string token1,
        string? token2,
        string? token3,
        string? token4,
        string? token5,
        VerifyLookupType? type);
}