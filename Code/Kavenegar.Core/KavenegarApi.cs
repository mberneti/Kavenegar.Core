using System.Net;
using Kavenegar.Core.Dto.Message;
using Kavenegar.Core.Dto.Result;
using Kavenegar.Core.Enums;
using Shared.Infrastructure;

namespace Kavenegar.Core;

internal class KavenegarApi : IKavenegarApi
{
    private readonly HttpClientHelper _httpClientHelper;

    public KavenegarApi(
        string apiKey)
    {
        _httpClientHelper = new HttpClientHelper
        {
            BaseAddress = $"https://api.kavenegar.com/v1/{apiKey}"
        };
    }

    public async Task<SendResult?> Send(
        SendSingleMessageRequest message,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "sender", message.MessageInfo.Sender
            },
            {
                "receptor", string.Join(',', message.ReceptorLocalMessageIds.Keys)
            },
            {
                "message", message.MessageInfo.Message
            },
            {
                "type", (int)message.MessageInfo.Type
            },
            {
                "date", message.Date?.ToUnixTimestamp() ?? 0
            }
        };

        if (message.ReceptorLocalMessageIds.Values.All(string.IsNullOrWhiteSpace))
            queryParams.Add("localId", string.Join(',', message.ReceptorLocalMessageIds.Values));

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/sendarray.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<SendResult>>>(cancellationToken))!.Value
            .FirstOrDefault();
    }

    public async Task<List<SendResult>> Send(
        SendMultiMessageRequest messages,
        CancellationToken cancellationToken = default)
    {
        var requestParams = new Dictionary<string, object?>
        {
            {
                "message",
                await messages.SendMessageInfos
                    .Select(sendMessageInfo => WebUtility.HtmlEncode(sendMessageInfo.MessageInfo.Message))
                    .ToList()
                    .Serialize(cancellationToken)
            },
            {
                "sender",
                await messages.SendMessageInfos.Select(sendMessageInfo => sendMessageInfo.MessageInfo.Sender)
                    .ToList()
                    .Serialize(cancellationToken)
            },
            {
                "receptor",
                await messages.SendMessageInfos.Select(sendMessageInfo => sendMessageInfo.Receptor)
                    .ToList()
                    .Serialize(cancellationToken)
            },
            {
                "type",
                await messages.SendMessageInfos.Select(sendMessageInfo => sendMessageInfo.MessageInfo.Type.ToString())
                    .Serialize(cancellationToken)
            },
            {
                "date", messages.Date == DateTime.MinValue ? 0 : messages.Date.ToUnixTimestamp()
            }
        };

        if (messages.SendMessageInfos.All(
                sendMessageInfo => !string.IsNullOrWhiteSpace(sendMessageInfo.LocalMessageId)))
            requestParams.Add(
                "localMessageIds",
                string.Join(
                    ",",
                    messages.SendMessageInfos.Select(
                        sendMessageInfo => !string.IsNullOrWhiteSpace(sendMessageInfo.LocalMessageId))));

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/send.json",
            null,
            requestParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<SendResult>>>(cancellationToken))!.Value;
    }

    public async Task<StatusMessageDto> Status(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        return (await Status(
            new List<string>
            {
                messageId
            },
            cancellationToken)).FirstOrDefault()!;
    }

    public async Task<List<StatusMessageDto>> Status(
        List<string> messageIds,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "messageid", string.Join(',', messageIds)
            }
        };

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/status.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<StatusMessageDto>>>(cancellationToken))?.Value ??
               new List<StatusMessageDto>();
    }

    public async Task<LocalStatusDto> StatusLocalMessageId(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        return (await StatusLocalMessageId(
            new List<string>
            {
                messageId
            },
            cancellationToken)).FirstOrDefault()!;
    }

    public async Task<List<LocalStatusDto>> StatusLocalMessageId(
        List<string> messageIds,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "messageid", string.Join(',', messageIds)
            }
        };

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/statuslocalmessageid.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<LocalStatusDto>>>(cancellationToken))?.Value ??
               new List<LocalStatusDto>();
    }

    public async Task<SendResult> Select(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        return (await Select(
            new List<string>
            {
                messageId
            },
            cancellationToken)).FirstOrDefault()!;
    }

    public async Task<List<SendResult>> Select(
        List<string> messageIds,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "messageid", string.Join(',', messageIds)
            }
        };

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/select.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<SendResult>>>(cancellationToken))?.Value ??
               new List<SendResult>();
    }

    public async Task<List<SendResult>> SelectOutbox(
        DateTime startDate,
        DateTime? endDate,
        string? sender,
        CancellationToken cancellationToken = default)
    {
        if (endDate <= startDate) throw new ArgumentException("تاریخ پایان باید از تاریخ شروع بزرگتر باشد.");

        if ((endDate - startDate)!.Value.TotalDays > 1)
            throw new ArgumentException(
                "حداکثر فاصله زمانی بین متغیر startDate تا متغیر endDate برابر با 1 روز می باشد.");

        if (startDate < DateTime.Now.AddDays(-60))
            throw new ArgumentException("تاریخ شروع startDate حداکثر باید تا 60 روز قبل باشد.");

        var queryParams = new Dictionary<string, object?>
        {
            {
                "startdate", startDate.ToUnixTimestamp()
            }
        };

        if (endDate.HasValue) queryParams.Add("enddate", endDate.Value.ToUnixTimestamp());

        if (string.IsNullOrWhiteSpace(sender)) queryParams.Add("sender", sender);

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/selectoutbox.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<SendResult>>>(cancellationToken))?.Value ??
               new List<SendResult>();
    }

    public async Task<List<SendResult>> LatestOutbox(
        long? pageSize,
        string? sender,
        CancellationToken cancellationToken = default)
    {
        if (pageSize is > 500) throw new ArgumentException("تعداد رکورد های خروجی این متد حداکثر 500 رکورد می‌باشد.");

        pageSize ??= 500;

        var queryParams = new Dictionary<string, object?>
        {
            {
                "pagesize", pageSize
            }
        };

        if (string.IsNullOrWhiteSpace(sender)) queryParams.Add("sender", sender);

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/latestoutbox.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<SendResult>>>(cancellationToken))?.Value ??
               new List<SendResult>();
    }

    public async Task<CountOutboxDto> CountOutbox(
        DateTime startDate,
        DateTime? endDate,
        int? status,
        CancellationToken cancellationToken = default)
    {
        if (endDate <= startDate) throw new ArgumentException("تاریخ پایان باید از تاریخ شروع بزرگتر باشد.");

        if ((endDate - startDate)!.Value.TotalDays > 1)
            throw new ArgumentException(
                "حداکثر فاصله زمانی بین متغیر startdate تا متغیر endDate برابر با 1 روز می باشد.");

        if (startDate < DateTime.Now.AddDays(-60))
            throw new ArgumentException("تاریخ شروع startdate حداکثر باید تا 60 روز قبل باشد.");

        var queryParams = new Dictionary<string, object?>
        {
            {
                "startdate", startDate.ToUnixTimestamp()
            }
        };

        if (endDate.HasValue) queryParams.Add("enddate", endDate.Value.ToUnixTimestamp());

        if (status.HasValue) queryParams.Add("status", status);

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/countoutbox.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<CountOutboxDto>>>(cancellationToken))?.Value
               .FirstOrDefault() ??
               new CountOutboxDto();
    }

    public async Task<StatusMessageDto> Cancel(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        return (await Cancel(
            new List<string>
            {
                messageId
            },
            cancellationToken)).FirstOrDefault()!;
    }

    public async Task<List<StatusMessageDto>> Cancel(
        List<string> ids,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "messageid", string.Join(',', ids)
            }
        };

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/cancel.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<StatusMessageDto>>>(cancellationToken))?.Value ??
               new List<StatusMessageDto>();
    }

    public async Task<List<ReceivedMessageDto>> Receive(
        string line,
        bool isRead,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "linenumber", line
            },
            {
                "isread", isRead ? 1 : 0
            }
        };

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/receive.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<ReceivedMessageDto>>>(cancellationToken))?.Value ??
               new List<ReceivedMessageDto>();
    }

    public async Task<CountInboxDto> CountInbox(
        DateTime startDate,
        DateTime? endDate,
        string? lineNumber,
        bool? isRead,
        CancellationToken cancellationToken = default)
    {
        if (endDate <= startDate) throw new ArgumentException("تاریخ پایان باید از تاریخ شروع بزرگتر باشد.");

        if ((endDate - startDate)!.Value.TotalDays > 1)
            throw new ArgumentException(
                "حداکثر فاصله زمانی بین متغیر startdate تا متغیر endDate برابر با 1 روز می باشد.");

        if (startDate < DateTime.Now.AddDays(-60))
            throw new ArgumentException("تاریخ شروع startdate حداکثر باید تا 60 روز قبل باشد.");

        var queryParams = new Dictionary<string, object?>
        {
            {
                "startdate", startDate.ToUnixTimestamp()
            }
        };

        if (endDate.HasValue) queryParams.Add("enddate", endDate.Value.ToUnixTimestamp());
        if (string.IsNullOrWhiteSpace(lineNumber)) queryParams.Add("linenumber", lineNumber);
        if (isRead.HasValue) queryParams.Add("isread", isRead.Value ? 1 : 0);

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/countinbox.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<CountInboxDto>>>(cancellationToken))?.Value
               .FirstOrDefault() ??
               new CountOutboxDto();
    }

    public async Task<AccountInfoDto> AccountInfo(
        CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "account/info.json",
            null,
            null,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<AccountInfoDto>>(cancellationToken))?.Value ??
               new AccountInfoDto();
    }

    public async Task<AccountConfigDto> AccountConfig(
        string? apiLogs,
        string? dailyReport,
        string? debugMode,
        string? defaultSender,
        int? minCreditAlarm,
        string? resendFailed,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>();

        if (string.IsNullOrWhiteSpace(apiLogs)) queryParams.Add("apilogs", apiLogs);

        if (string.IsNullOrWhiteSpace(dailyReport)) queryParams.Add("dailyreport", dailyReport);

        if (string.IsNullOrWhiteSpace(debugMode)) queryParams.Add("debugmode", debugMode);

        if (string.IsNullOrWhiteSpace(defaultSender)) queryParams.Add("defaultsender", defaultSender);

        queryParams.Add("mincreditalarm", minCreditAlarm);

        if (string.IsNullOrWhiteSpace(resendFailed)) queryParams.Add("resendfailed", resendFailed);

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "account/config.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<AccountConfigDto>>(cancellationToken))?.Value ??
               new AccountConfigDto();
    }

    public async Task<SendResult> VerifyLookup(
        string receptor,
        string template,
        string token1,
        string? token2 = null,
        string? token3 = null,
        string? token4 = null,
        string? token5 = null,
        VerifyLookupType? type = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "receptor", receptor
            },
            {
                "template", template
            },
            {
                "token", token1
            }
        };

        if (string.IsNullOrWhiteSpace(token2)) queryParams.Add("token2", token2);

        if (string.IsNullOrWhiteSpace(token3)) queryParams.Add("token3", token3);

        if (string.IsNullOrWhiteSpace(token4)) queryParams.Add("token10", token4);

        if (string.IsNullOrWhiteSpace(token5)) queryParams.Add("token20", token5);

        if (type.HasValue) queryParams.Add("type", type.Value.ToString());

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "verify/lookup.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<SendResult>>>(cancellationToken))?.Value
               .FirstOrDefault() ??
               new SendResult();
    }
}