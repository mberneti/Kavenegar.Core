using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kavenegar.Core.Models;
using Kavenegar.Core.Models.Enums;

namespace Kavenegar.Core;

class KavenegarApi2 : IKavenegarApi
{
    public KavenegarApi2(
        string apiKey)
    {
        ApiKey = apiKey;
    }

    public string ApiKey { get; set; }

    public Task<List<SendResult>> Send(
        string sender,
        List<string> receptor,
        string message)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> Send(
        string sender,
        string receptor,
        string message)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> Send(
        string sender,
        string receptor,
        string message,
        MessageType type,
        DateTime date)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> Send(
        string sender,
        List<string> receptor,
        string message,
        MessageType type,
        DateTime date)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> Send(
        string sender,
        string receptor,
        string message,
        MessageType type,
        DateTime date,
        string localId)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> Send(
        string sender,
        string receptor,
        string message,
        string localId)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> Send(
        string sender,
        List<string> receptors,
        string message,
        string localId)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> Send(
        string sender,
        List<string> receptor,
        string message,
        MessageType type,
        DateTime date,
        List<string> localIds)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SendArray(
        List<string> senders,
        List<string> receptors,
        List<string> messages)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SendArray(
        string sender,
        List<string> receptors,
        List<string> messages,
        MessageType type,
        DateTime date)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SendArray(
        string sender,
        List<string> receptors,
        List<string> messages,
        MessageType type,
        DateTime date,
        string localMessageIds)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SendArray(
        string sender,
        List<string> receptors,
        List<string> messages,
        string localMessageId)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SendArray(
        List<string> senders,
        List<string> receptors,
        List<string> messages,
        string localMessageId)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SendArray(
        List<string> senders,
        List<string> receptors,
        List<string> messages,
        List<MessageType> types,
        DateTime date,
        List<string> localMessageIds)
    {
        throw new NotImplementedException();
    }

    public Task<List<StatusResult>> Status(
        List<string> messageIds)
    {
        throw new NotImplementedException();
    }

    public Task<StatusResult> Status(
        string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<List<StatusLocalMessageIdResult>> StatusLocalMessageId(
        List<string> messageIds)
    {
        throw new NotImplementedException();
    }

    public Task<StatusLocalMessageIdResult> StatusLocalMessageId(
        string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> Select(
        List<string> messageIds)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> Select(
        string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SelectOutbox(
        DateTime startDate)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SelectOutbox(
        DateTime startDate,
        DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SelectOutbox(
        DateTime startDate,
        DateTime endDate,
        string sender)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> LatestOutbox(
        long pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> LatestOutbox(
        long pageSize,
        string sender)
    {
        throw new NotImplementedException();
    }

    public Task<CountOutboxResult> CountOutbox(
        DateTime startDate)
    {
        throw new NotImplementedException();
    }

    public Task<CountOutboxResult> CountOutbox(
        DateTime startDate,
        DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<CountOutboxResult> CountOutbox(
        DateTime startDate,
        DateTime endDate,
        int status)
    {
        throw new NotImplementedException();
    }

    public Task<List<StatusResult>> Cancel(
        List<string> ids)
    {
        throw new NotImplementedException();
    }

    public Task<StatusResult> Cancel(
        string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ReceiveResult>> Receive(
        string line,
        int isRead)
    {
        throw new NotImplementedException();
    }

    public Task<CountInboxResult> CountInbox(
        DateTime startDate,
        string lineNumber)
    {
        throw new NotImplementedException();
    }

    public Task<CountInboxResult> CountInbox(
        DateTime startDate,
        DateTime endDate,
        string lineNumber)
    {
        throw new NotImplementedException();
    }

    public Task<CountInboxResult> CountInbox(
        DateTime startDate,
        DateTime endDate,
        string lineNumber,
        int isRead)
    {
        throw new NotImplementedException();
    }

    public Task<List<CountPostalCodeResult>> CountPostalCode(
        long postalcode)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SendByPostalCode(
        long postalcode,
        string sender,
        string message,
        long mciStartIndex,
        long mciCount,
        long mtnStartIndex,
        long mtnCount)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> SendByPostalCode(
        long postalcode,
        string sender,
        string message,
        long mciStartIndex,
        long mciCount,
        long mtnStartIndex,
        long mtnCount,
        DateTime date)
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
        string resendFailed)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string template)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string template,
        VerifyLookupType type)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string template)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string token10,
        string template)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string template,
        VerifyLookupType type)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string token10,
        string template,
        VerifyLookupType type)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string token10,
        string token20,
        string template,
        VerifyLookupType type)
    {
        throw new NotImplementedException();
    }

    public Task<SendResult> CallMakeTts(
        string message,
        string receptor)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> CallMakeTts(
        string message,
        List<string> receptor)
    {
        throw new NotImplementedException();
    }

    public Task<List<SendResult>> CallMakeTts(
        string message,
        List<string> receptor,
        DateTime? date,
        List<string> localId)
    {
        throw new NotImplementedException();
    }
}