using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kavenegar.Core.Models;
using Kavenegar.Core.Models.Enums;

namespace Kavenegar.Core;

public interface IKavenegarApi
{
    string ApiKey { set; get; }

    Task<List<SendResult>> Send(
        string sender,
        List<string> receptor,
        string message);

    Task<SendResult> Send(
        string sender,
        string receptor,
        string message);

    Task<SendResult> Send(
        string sender,
        string receptor,
        string message,
        MessageType type,
        DateTime date);

    Task<List<SendResult>> Send(
        string sender,
        List<string> receptor,
        string message,
        MessageType type,
        DateTime date);

    Task<SendResult> Send(
        string sender,
        string receptor,
        string message,
        MessageType type,
        DateTime date,
        string localId);

    Task<SendResult> Send(
        string sender,
        string receptor,
        string message,
        string localId);

    Task<List<SendResult>> Send(
        string sender,
        List<string> receptors,
        string message,
        string localId);

    Task<List<SendResult>> Send(
        string sender,
        List<string> receptor,
        string message,
        MessageType type,
        DateTime date,
        List<string> localIds);

    Task<List<SendResult>> SendArray(
        List<string> senders,
        List<string> receptors,
        List<string> messages);

    Task<List<SendResult>> SendArray(
        string sender,
        List<string> receptors,
        List<string> messages,
        MessageType type,
        DateTime date);

    Task<List<SendResult>> SendArray(
        string sender,
        List<string> receptors,
        List<string> messages,
        MessageType type,
        DateTime date,
        string localMessageIds);

    Task<List<SendResult>> SendArray(
        string sender,
        List<string> receptors,
        List<string> messages,
        string localMessageId);

    Task<List<SendResult>> SendArray(
        List<string> senders,
        List<string> receptors,
        List<string> messages,
        string localMessageId);

    Task<List<SendResult>> SendArray(
        List<string> senders,
        List<string> receptors,
        List<string> messages,
        List<MessageType> types,
        DateTime date,
        List<string> localMessageIds);

    Task<List<StatusResult>> Status(
        List<string> messageIds);

    Task<StatusResult> Status(
        string messageId);

    Task<List<StatusLocalMessageIdResult>> StatusLocalMessageId(
        List<string> messageIds);

    Task<StatusLocalMessageIdResult> StatusLocalMessageId(
        string messageId);

    Task<List<SendResult>> Select(
        List<string> messageIds);

    Task<SendResult> Select(
        string messageId);

    Task<List<SendResult>> SelectOutbox(
        DateTime startDate);

    Task<List<SendResult>> SelectOutbox(
        DateTime startDate,
        DateTime endDate);

    Task<List<SendResult>> SelectOutbox(
        DateTime startDate,
        DateTime endDate,
        string sender);

    Task<List<SendResult>> LatestOutbox(
        long pageSize);

    Task<List<SendResult>> LatestOutbox(
        long pageSize,
        string sender);

    Task<CountOutboxResult> CountOutbox(
        DateTime startDate);

    Task<CountOutboxResult> CountOutbox(
        DateTime startDate,
        DateTime endDate);

    Task<CountOutboxResult> CountOutbox(
        DateTime startDate,
        DateTime endDate,
        int status);

    Task<List<StatusResult>> Cancel(
        List<string> ids);

    Task<StatusResult> Cancel(
        string messageId);

    Task<List<ReceiveResult>> Receive(
        string line,
        int isRead);

    Task<CountInboxResult> CountInbox(
        DateTime startDate,
        string lineNumber);

    Task<CountInboxResult> CountInbox(
        DateTime startDate,
        DateTime endDate,
        string lineNumber);

    Task<CountInboxResult> CountInbox(
        DateTime startDate,
        DateTime endDate,
        string lineNumber,
        int isRead);

    Task<List<CountPostalCodeResult>> CountPostalCode(
        long postalcode);

    Task<List<SendResult>> SendByPostalCode(
        long postalcode,
        string sender,
        string message,
        long mciStartIndex,
        long mciCount,
        long mtnStartIndex,
        long mtnCount);

    Task<List<SendResult>> SendByPostalCode(
        long postalcode,
        string sender,
        string message,
        long mciStartIndex,
        long mciCount,
        long mtnStartIndex,
        long mtnCount,
        DateTime date);

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
        string token,
        string template);

    Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string template,
        VerifyLookupType type);

    Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string template);

    Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string token10,
        string template);

    Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string template,
        VerifyLookupType type);

    Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string token10,
        string template,
        VerifyLookupType type);

    Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string token10,
        string token20,
        string template,
        VerifyLookupType type);

    Task<SendResult> CallMakeTts(
        string message,
        string receptor);

    Task<List<SendResult>> CallMakeTts(
        string message,
        List<string> receptor);

    Task<List<SendResult>> CallMakeTts(
        string message,
        List<string> receptor,
        DateTime? date,
        List<string> localId);
}