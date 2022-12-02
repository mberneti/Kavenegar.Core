using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Kavenegar.Core.Exceptions;
using Kavenegar.Core.Models;
using Kavenegar.Core.Models.Enums;
using Kavenegar.Core.Utils;
using Newtonsoft.Json;

namespace Kavenegar.Core;

public class KavenegarApi : IKavenegarApi
{
    private const string ApiPath = "{0}/{1}.{2}";
    private const string BaseUrl = "http://api.kavenegar.com/v1";
    private static HttpClient _client = null!;

    public KavenegarApi(
        string apikey)
    {
        ApiKey = apikey;

        _client = new HttpClient
        {
            BaseAddress = new Uri($"{BaseUrl}/{ApiKey}/")
        };
    }

    public string ApiKey { set; get; }

    public async Task<List<SendResult>> Send(
        string sender,
        List<string> receptor,
        string message)
    {
        return await Send(
            sender,
            receptor,
            message,
            MessageType.MobileMemory,
            DateTime.MinValue);
    }

    public async Task<SendResult> Send(
        string sender,
        string receptor,
        string message)
    {
        return await Send(
            sender,
            receptor,
            message,
            MessageType.MobileMemory,
            DateTime.MinValue);
    }

    public async Task<SendResult> Send(
        string sender,
        string receptor,
        string message,
        MessageType type,
        DateTime date)
    {
        var receptors = new List<string>
        {
            receptor
        };
        return (await Send(
            sender,
            receptors,
            message,
            type,
            date))[0];
    }

    public async Task<List<SendResult>> Send(
        string sender,
        List<string> receptor,
        string message,
        MessageType type,
        DateTime date)
    {
        return await Send(
            sender,
            receptor,
            message,
            type,
            date,
            null);
    }

    public async Task<SendResult> Send(
        string sender,
        string receptor,
        string message,
        MessageType type,
        DateTime date,
        string localId)
    {
        var receptors = new List<string>
        {
            receptor
        };
        var localIds = new List<string>
        {
            localId
        };
        return (await Send(
            sender,
            receptors,
            message,
            type,
            date,
            localIds))[0];
    }

    public async Task<SendResult> Send(
        string sender,
        string receptor,
        string message,
        string localId)
    {
        return await Send(
            sender,
            receptor,
            message,
            MessageType.MobileMemory,
            DateTime.MinValue,
            localId);
    }

    public async Task<List<SendResult>> Send(
        string sender,
        List<string> receptors,
        string message,
        string localId)
    {
        var localIds = new List<string>();
        for (var i = 0; i <= receptors.Count - 1; i++) localIds.Add(localId);
        return await Send(
            sender,
            receptors,
            message,
            MessageType.MobileMemory,
            DateTime.MinValue,
            localIds);
    }

    public async Task<List<SendResult>> Send(
        string sender,
        List<string> receptor,
        string message,
        MessageType type,
        DateTime date,
        List<string> localIds)
    {
        var path = GetApiPath(
            "sms",
            "send",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "sender", WebUtility.HtmlEncode(sender)
            },
            {
                "receptor", WebUtility.HtmlEncode(string.Join(",", receptor))
            },
            {
                "message", message
            },
            {
                "type", (int)type
            },
            {
                "date", date == DateTime.MinValue ? 0 : date.ToUnixTimestamp()
            }
        };
        if (localIds != null &&
            localIds.Count > 0)
            param.Add("localId", string.Join(",", localIds));
        var responseBody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnSend>(responseBody);
        return l.entries;
    }

    public async Task<List<SendResult>> SendArray(
        List<string> senders,
        List<string> receptors,
        List<string> messages)
    {
        var types = new List<MessageType>();
        for (var i = 0; i <= senders.Count - 1; i++) types.Add(MessageType.MobileMemory);
        return await SendArray(
            senders,
            receptors,
            messages,
            types,
            DateTime.MinValue,
            null);
    }

    public async Task<List<SendResult>> SendArray(
        string sender,
        List<string> receptors,
        List<string> messages,
        MessageType type,
        DateTime date)
    {
        var senders = new List<string>();
        for (var i = 0; i < receptors.Count; i++) senders.Add(sender);
        var types = new List<MessageType>();
        for (var i = 0; i <= senders.Count - 1; i++) types.Add(MessageType.MobileMemory);
        return await SendArray(
            senders,
            receptors,
            messages,
            types,
            date,
            null);
    }

    public async Task<List<SendResult>> SendArray(
        string sender,
        List<string> receptors,
        List<string> messages,
        MessageType type,
        DateTime date,
        string localMessageIds)
    {
        var senders = new List<string>();
        for (var i = 0; i < receptors.Count; i++) senders.Add(sender);
        var types = new List<MessageType>();
        for (var i = 0; i <= senders.Count - 1; i++) types.Add(MessageType.MobileMemory);
        return await SendArray(
            senders,
            receptors,
            messages,
            types,
            date,
            new List<string>
            {
                localMessageIds
            });
    }

    public async Task<List<SendResult>> SendArray(
        string sender,
        List<string> receptors,
        List<string> messages,
        string localMessageId)
    {
        var senders = new List<string>();
        for (var i = 0; i < receptors.Count; i++) senders.Add(sender);

        return await SendArray(
            senders,
            receptors,
            messages,
            localMessageId);
    }

    public async Task<List<SendResult>> SendArray(
        List<string> senders,
        List<string> receptors,
        List<string> messages,
        string localMessageId)
    {
        var types = new List<MessageType>();
        for (var i = 0; i <= receptors.Count - 1; i++) types.Add(MessageType.MobileMemory);
        var localMessageIds = new List<string>();
        for (var i = 0; i <= receptors.Count - 1; i++) localMessageIds.Add(localMessageId);
        return await SendArray(
            senders,
            receptors,
            messages,
            types,
            DateTime.MinValue,
            localMessageIds);
    }

    public async Task<List<SendResult>> SendArray(
        List<string> senders,
        List<string> receptors,
        List<string> messages,
        List<MessageType> types,
        DateTime date,
        List<string> localMessageIds)
    {
        var path = GetApiPath(
            "sms",
            "sendarray",
            "json");
        var jsonSenders = JsonConvert.SerializeObject(senders);
        var jsonReceptors = JsonConvert.SerializeObject(receptors);
        var jsonMessages = JsonConvert.SerializeObject(messages);
        var jsonTypes = JsonConvert.SerializeObject(types);
        var param = new Dictionary<string, object>
        {
            {
                "message", jsonMessages
            },
            {
                "sender", jsonSenders
            },
            {
                "receptor", jsonReceptors
            },
            {
                "type", jsonTypes
            },
            {
                "date", date == DateTime.MinValue ? 0 : date.ToUnixTimestamp()
            }
        };
        if (localMessageIds != null &&
            localMessageIds.Count > 0)
            param.Add("localMessageIds", string.Join(",", localMessageIds));

        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
        if (l.entries == null) return new List<SendResult>();
        return l.entries;
    }

    public async Task<List<StatusResult>> Status(
        List<string> messageIds)
    {
        var path = GetApiPath(
            "sms",
            "status",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "messageId", string.Join(",", messageIds)
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnStatus>(responsebody);
        if (l.entries == null) return new List<StatusResult>();
        return l.entries;
    }

    public async Task<StatusResult> Status(
        string messageId)
    {
        var ids = new List<string>
        {
            messageId
        };
        var result = await Status(ids);
        return result.Count == 1 ? result[0] : null;
    }

    public async Task<List<StatusLocalMessageIdResult>> StatusLocalMessageId(
        List<string> messageIds)
    {
        var path = GetApiPath(
            "sms",
            "statuslocalmessageid",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "localId", string.Join(",", messageIds)
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnStatusLocalMessageId>(responsebody);
        return l.entries;
    }

    public async Task<StatusLocalMessageIdResult> StatusLocalMessageId(
        string messageId)
    {
        var result = await StatusLocalMessageId(
            new List<string>
            {
                messageId
            });
        return result.Count == 1 ? result[0] : null;
    }

    public async Task<List<SendResult>> Select(
        List<string> messageIds)
    {
        var path = GetApiPath(
            "sms",
            "select",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "messageId", string.Join(",", messageIds)
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
        if (l.entries == null) return new List<SendResult>();
        return l.entries;
    }

    public async Task<SendResult> Select(
        string messageId)
    {
        var ids = new List<string>
        {
            messageId
        };
        var result = await Select(ids);
        return result.Count == 1 ? result[0] : null;
    }

    public async Task<List<SendResult>> SelectOutbox(
        DateTime startDate)
    {
        return await SelectOutbox(startDate, DateTime.MaxValue);
    }

    public async Task<List<SendResult>> SelectOutbox(
        DateTime startDate,
        DateTime endDate)
    {
        return await SelectOutbox(
            startDate,
            endDate,
            null);
    }

    public async Task<List<SendResult>> SelectOutbox(
        DateTime startDate,
        DateTime endDate,
        string sender)
    {
        var path = GetApiPath(
            "sms",
            "selectoutbox",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "startDate", startDate == DateTime.MinValue ? 0 : startDate.ToUnixTimestamp()
            },
            {
                "endDate", endDate == DateTime.MinValue ? 0 : endDate.ToUnixTimestamp()
            },
            {
                "sender", sender
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
        return l.entries;
    }

    public async Task<List<SendResult>> LatestOutbox(
        long pageSize)
    {
        return await LatestOutbox(pageSize, "");
    }

    public async Task<List<SendResult>> LatestOutbox(
        long pageSize,
        string sender)
    {
        var path = GetApiPath(
            "sms",
            "latestoutbox",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "pageSize", pageSize
            },
            {
                "sender", sender
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
        return l.entries;
    }

    public async Task<CountOutboxResult> CountOutbox(
        DateTime startDate)
    {
        return await CountOutbox(
            startDate,
            DateTime.MaxValue,
            10);
    }

    public async Task<CountOutboxResult> CountOutbox(
        DateTime startDate,
        DateTime endDate)
    {
        return await CountOutbox(
            startDate,
            endDate,
            0);
    }

    public async Task<CountOutboxResult> CountOutbox(
        DateTime startDate,
        DateTime endDate,
        int status)
    {
        var path = GetApiPath(
            "sms",
            "countoutbox",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "startDate", startDate == DateTime.MinValue ? 0 : startDate.ToUnixTimestamp()
            },
            {
                "endDate", endDate == DateTime.MinValue ? 0 : endDate.ToUnixTimestamp()
            },
            {
                "status", status
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnCountOutbox>(responsebody);
        if (l.entries == null ||
            l.entries[0] == null)
            return new CountOutboxResult();
        return l.entries[0];
    }

    public async Task<List<StatusResult>> Cancel(
        List<string> ids)
    {
        var path = GetApiPath(
            "sms",
            "cancel",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "messageId", string.Join(",", ids)
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnStatus>(responsebody);
        return l.entries;
    }

    public async Task<StatusResult> Cancel(
        string messageId)
    {
        var ids = new List<string>
        {
            messageId
        };
        var result = await Cancel(ids);
        return result.Count == 1 ? result[0] : null;
    }

    public async Task<List<ReceiveResult>> Receive(
        string line,
        int isRead)
    {
        var path = GetApiPath(
            "sms",
            "receive",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "lineNumber", line
            },
            {
                "isRead", isRead
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnReceive>(responsebody);
        if (l.entries == null) return new List<ReceiveResult>();
        return l.entries;
    }

    public async Task<CountInboxResult> CountInbox(
        DateTime startDate,
        string lineNumber)
    {
        return await CountInbox(
            startDate,
            DateTime.MaxValue,
            lineNumber,
            0);
    }

    public async Task<CountInboxResult> CountInbox(
        DateTime startDate,
        DateTime endDate,
        string lineNumber)
    {
        return await CountInbox(
            startDate,
            endDate,
            lineNumber,
            0);
    }

    public async Task<CountInboxResult> CountInbox(
        DateTime startDate,
        DateTime endDate,
        string lineNumber,
        int isRead)
    {
        var path = GetApiPath(
            "sms",
            "countoutbox",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "startDate", startDate == DateTime.MinValue ? 0 : startDate.ToUnixTimestamp()
            },
            {
                "endDate", endDate == DateTime.MinValue ? 0 : endDate.ToUnixTimestamp()
            },
            {
                "lineNumber", lineNumber
            },
            {
                "isRead", isRead
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnCountInbox>(responsebody);
        return l.entries[0];
    }

    public async Task<List<CountPostalCodeResult>> CountPostalCode(
        long postalcode)
    {
        var path = GetApiPath(
            "sms",
            "countpostalcode",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "postalcode", postalcode
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnCountPostalCode>(responsebody);
        return l.entries;
    }

    public async Task<List<SendResult>> SendByPostalCode(
        long postalcode,
        string sender,
        string message,
        long mciStartIndex,
        long mciCount,
        long mtnStartIndex,
        long mtnCount)
    {
        return await SendByPostalCode(
            postalcode,
            sender,
            message,
            mciStartIndex,
            mciCount,
            mtnStartIndex,
            mtnCount,
            DateTime.MinValue);
    }

    public async Task<List<SendResult>> SendByPostalCode(
        long postalcode,
        string sender,
        string message,
        long mciStartIndex,
        long mciCount,
        long mtnStartIndex,
        long mtnCount,
        DateTime date)
    {
        var path = GetApiPath(
            "sms",
            "sendbypostalcode",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "postalcode", postalcode
            },
            {
                "sender", sender
            },
            {
                "message", WebUtility.HtmlEncode(message)
            },
            {
                "mciStartIndex", mciStartIndex
            },
            {
                "mciCount", mciCount
            },
            {
                "mtnStartIndex", mtnStartIndex
            },
            {
                "mtnCount", mtnCount
            },
            {
                "date", date == DateTime.MinValue ? 0 : date.ToUnixTimestamp()
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
        return l.entries;
    }

    public async Task<AccountInfoResult> AccountInfo()
    {
        var path = GetApiPath(
            "account",
            "info",
            "json");
        var responsebody = await Execute(path, null);
        var l = JsonConvert.DeserializeObject<ReturnAccountInfo>(responsebody);
        return l.entries;
    }

    public async Task<AccountConfigResult> AccountConfig(
        string apiLogs,
        string dailyReport,
        string debugMode,
        string defaultSender,
        int? minCreditAlarm,
        string resendFailed)
    {
        var path = GetApiPath(
            "account",
            "config",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "apiLogs", apiLogs
            },
            {
                "dailyReport", dailyReport
            },
            {
                "debugMode", debugMode
            },
            {
                "defaultSender", defaultSender
            },
            {
                "minCreditAlarm", minCreditAlarm
            },
            {
                "resendFailed", resendFailed
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnAccountConfig>(responsebody);
        return l.entries;
    }

    public async Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string template)
    {
        return await VerifyLookup(
            receptor,
            token,
            null,
            null,
            template,
            VerifyLookupType.Sms);
    }

    public async Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string template,
        VerifyLookupType type)
    {
        return await VerifyLookup(
            receptor,
            token,
            null,
            null,
            template,
            type);
    }

    public async Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string template)
    {
        return await VerifyLookup(
            receptor,
            token,
            token2,
            token3,
            template,
            VerifyLookupType.Sms);
    }

    public async Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string token10,
        string template)
    {
        return await VerifyLookup(
            receptor,
            token,
            token2,
            token3,
            token10,
            template,
            VerifyLookupType.Sms);
    }

    public async Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string template,
        VerifyLookupType type)
    {
        return await VerifyLookup(
            receptor,
            token,
            token2,
            token3,
            null,
            template,
            type);
    }

    public async Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string token10,
        string template,
        VerifyLookupType type)
    {
        return await VerifyLookup(
            receptor,
            token,
            token2,
            token3,
            token10,
            null,
            template,
            type);
    }

    public async Task<SendResult> VerifyLookup(
        string receptor,
        string token,
        string token2,
        string token3,
        string token10,
        string token20,
        string template,
        VerifyLookupType type)
    {
        var path = GetApiPath(
            "verify",
            "lookup",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "receptor", receptor
            },
            {
                "template", template
            },
            {
                "token", token
            },
            {
                "token2", token2
            },
            {
                "token3", token3
            },
            {
                "token10", token10
            },
            {
                "token20", token20
            },
            {
                "type", type
            }
        };
        var responsebody = await Execute(path, param);
        var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
        return l.entries[0];
    }

    private string GetApiPath(
        string @base,
        string method,
        string output)
    {
        return string.Format(
            ApiPath,
            @base,
            method,
            output);
    }

    private async Task<string> Execute(
        string path,
        Dictionary<string, object> @params)
    {
        var nvc = @params?.Select(x => new KeyValuePair<string, string>(x.Key, x.Value?.ToString()));

        var postdata = new FormUrlEncodedContent(nvc);

        var response = await _client.PostAsync(path, postdata);
        var responseBody = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<ReturnResult>(responseBody);

        if (response.StatusCode != HttpStatusCode.OK)
            throw new ApiException(result.Return.Message, result.Return.Status);

        return responseBody;
    }


    #region << CallMakeTts >>

    public async Task<SendResult> CallMakeTts(
        string message,
        string receptor)
    {
        return (await CallMakeTts(
            message,
            new List<string>
            {
                receptor
            },
            null,
            null))[0];
    }

    public async Task<List<SendResult>> CallMakeTts(
        string message,
        List<string> receptor)
    {
        return await CallMakeTts(
            message,
            receptor,
            null,
            null);
    }

    public async Task<List<SendResult>> CallMakeTts(
        string message,
        List<string> receptor,
        DateTime? date,
        List<string> localId)
    {
        var path = GetApiPath(
            "call",
            "maketts",
            "json");
        var param = new Dictionary<string, object>
        {
            {
                "receptor", string.Join(",", receptor)
            },
            {
                "message", WebUtility.HtmlEncode(message)
            }
        };
        if (date != null) param.Add("date", date.Value.ToUnixTimestamp());
        if (localId != null &&
            localId.Count > 0)
            param.Add("localId", string.Join(",", localId));
        var responseBody = await Execute(path, param);

        return JsonConvert.DeserializeObject<ReturnSend>(responseBody).entries;
    }

    #endregion << CallMakeTts >>
}