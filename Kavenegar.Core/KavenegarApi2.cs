using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kavenegar.Core.Models;
using Kavenegar.Core.Models.Enums;

namespace Kavenegar.Core;

class KavenegarApi2 : IKavenegarApi2
{
    public string ApiKey { get; set; } = null!;

    public Task<List<SendResult>> Send(
        SendMessageRequest message,
        DateTime? date
    )
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SendArray(
        List<SendMessageRequest> messages,
        DateTime? date
    )
    {
        throw new NotImplementedException();
    }

    public Task<StatusResult> Status(string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<List<StatusResult>> Status(List<string> messageIds)
    {
        throw new NotImplementedException();
    }

    public Task<StatusLocalMessageIdResult> StatusLocalMessageId(string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<List<StatusLocalMessageIdResult>> StatusLocalMessageId(List<string> messageIds)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> Select(string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> Select(List<string> messageIds)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SelectOutbox(
        DateTime startDate,
        DateTime? endDate,
        string? sender
    )
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> LatestOutbox(
        long? pageSize,
        string? sender
    )
    {
        throw new NotImplementedException();
    }

    public Task<CountOutboxResult> CountOutbox(
        DateTime startDate,
        DateTime? endDate,
        int? status
    )
    {
        throw new NotImplementedException();
    }

    public Task<StatusResult> Cancel(string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<List<StatusResult>> Cancel(List<string> ids)
    {
        throw new NotImplementedException();
    }

    public Task<List<ReceiveResult>> Receive(
        string line,
        int isRead
    )
    {
        throw new NotImplementedException();
    }

    public Task<CountInboxResult> CountInbox(
        DateTime startDate,
        DateTime? endDate,
        string? lineNumber,
        int? isRead
    )
    {
        throw new NotImplementedException();
    }

    public Task<AccountInfoResult> AccountInfo()
    {
        throw new NotImplementedException();
    }

    public Task<AccountConfigResult> AccountConfig(
        string apiLogs,
        string dailyReport,
        string debugMode,
        string defaultSender,
        int? minCreditAlarm,
        string resendFailed
    )
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> VerifyLookup(
        string receptor,
        string template,
        string token1,
        string? token2,
        string? token3,
        string? token4,
        string? token5,
        VerifyLookupType? type
    )
    {
        throw new NotImplementedException();
    }
}