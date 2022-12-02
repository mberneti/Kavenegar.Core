using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kavenegar.Core.Dto;
using Kavenegar.Core.Models;
using Kavenegar.Core.Models.Enums;
using Kavenegar.Core.Utils;
using Shared.Infrastructure;

namespace Kavenegar.Core;

internal class KavenegarApi2 : IKavenegarApi2
{
    private readonly HttpClientHelper _httpClientHelper;
    private readonly IMapper _mapper;

    public KavenegarApi2(
        string apiKey,
        IMapper mapper)
    {
        _mapper = mapper;
        _httpClientHelper = new HttpClientHelper
        {
            BaseAddress = $"https://api.kavenegar.com/v1/{apiKey}"
        };
    }

    public async Task<SendResult?> Send(
        SendSingleMessageRequest message,
        CancellationToken cancellationToken = default)
    {
        var requestParams = _mapper.Map<SendSingleMessageRequestDto>(message).ParametrizeObject();

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/send.json",
            null,
            requestParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<SendResult>>>(cancellationToken))!.Value
            .FirstOrDefault();
    }

    public async Task<List<SendResult>> Send(
        SendMultiMessageRequest messages,
        CancellationToken cancellationToken = default)
    {
        var requestParams = _mapper.Map<SendMultiMessageRequestDto>(messages).ParametrizeObject();

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/send.json",
            null,
            requestParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<SendResult>>>(cancellationToken))!.Value;
    }

    public async Task<StatusResult> Status(
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

    public async Task<List<StatusResult>> Status(
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

        return (await httpResponseMessage.Deserialize<ResultDto<List<StatusResult>>>(cancellationToken))?.Value ??
               new List<StatusResult>();
    }

    public async Task<StatusLocalMessageIdResult> StatusLocalMessageId(
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

    public async Task<List<StatusLocalMessageIdResult>> StatusLocalMessageId(
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

        return (await httpResponseMessage.Deserialize<ResultDto<List<StatusLocalMessageIdResult>>>(cancellationToken))
               ?.Value ??
               new List<StatusLocalMessageIdResult>();
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
        if (endDate <= startDate) throw new ArgumentException("تاریخ پایان نباید از تاریخ شروع کوچکتر باشد.");

        if ((endDate - startDate)!.Value.TotalDays > 1)
            throw new ArgumentException(
                "حداکثر فاصله زمانی بین متغیر startdate تا متغیر enddate برابر با 1 روز می باشد.");

        if (startDate < DateTime.Now.AddDays(-60))
            throw new ArgumentException("تاریخ شروع startdate حداکثر باید تا 60 روز قبل باشد.");

        var queryParams = new Dictionary<string, object?>
        {
            {
                "startdate", startDate.ToUnixTimestamp()
            }
        };

        if (endDate.HasValue) queryParams.Add("endate", endDate.Value.ToUnixTimestamp());

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

    public async Task<CountOutboxResult> CountOutbox(
        DateTime startDate,
        DateTime? endDate,
        int? status,
        CancellationToken cancellationToken = default)
    {
        if (endDate <= startDate) throw new ArgumentException("تاریخ پایان نباید از تاریخ شروع کوچکتر باشد.");

        if ((endDate - startDate)!.Value.TotalDays > 1)
            throw new ArgumentException(
                "حداکثر فاصله زمانی بین متغیر startdate تا متغیر enddate برابر با 1 روز می باشد.");

        if (startDate < DateTime.Now.AddDays(-60))
            throw new ArgumentException("تاریخ شروع startdate حداکثر باید تا 60 روز قبل باشد.");

        var queryParams = new Dictionary<string, object?>
        {
            {
                "startdate", startDate.ToUnixTimestamp()
            }
        };

        if (endDate.HasValue) queryParams.Add("endate", endDate.Value.ToUnixTimestamp());

        if (status.HasValue) queryParams.Add("status", status);

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/countoutbox.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<CountOutboxResult>>>(cancellationToken))?.Value
               .FirstOrDefault() ??
               new CountOutboxResult();
    }

    public async Task<StatusResult> Cancel(
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

    public async Task<List<StatusResult>> Cancel(
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

        return (await httpResponseMessage.Deserialize<ResultDto<List<StatusResult>>>(cancellationToken))?.Value ??
               new List<StatusResult>();
    }

    public async Task<List<ReceiveResult>> Receive(
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

        return (await httpResponseMessage.Deserialize<ResultDto<List<ReceiveResult>>>(cancellationToken))?.Value ??
               new List<ReceiveResult>();
    }

    public async Task<CountInboxResult> CountInbox(
        DateTime startDate,
        DateTime? endDate,
        string? lineNumber,
        bool? isRead,
        CancellationToken cancellationToken = default)
    {
        if (endDate <= startDate) throw new ArgumentException("تاریخ پایان نباید از تاریخ شروع کوچکتر باشد.");

        if ((endDate - startDate)!.Value.TotalDays > 1)
            throw new ArgumentException(
                "حداکثر فاصله زمانی بین متغیر startdate تا متغیر enddate برابر با 1 روز می باشد.");

        if (startDate < DateTime.Now.AddDays(-60))
            throw new ArgumentException("تاریخ شروع startdate حداکثر باید تا 60 روز قبل باشد.");

        var queryParams = new Dictionary<string, object?>
        {
            {
                "startdate", startDate.ToUnixTimestamp()
            }
        };

        if (endDate.HasValue) queryParams.Add("endate", endDate.Value.ToUnixTimestamp());
        if (string.IsNullOrWhiteSpace(lineNumber)) queryParams.Add("linenumber", lineNumber);
        if (isRead.HasValue) queryParams.Add("isread", isRead.Value ? 1 : 0);

        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "sms/countinbox.json",
            null,
            queryParams,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<List<CountInboxResult>>>(cancellationToken))?.Value
               .FirstOrDefault() ??
               new CountOutboxResult();
    }

    public async Task<AccountInfoResult> AccountInfo(
        CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await _httpClientHelper.PostAsync(
            "account/info.json",
            null,
            null,
            cancellationToken);

        return (await httpResponseMessage.Deserialize<ResultDto<AccountInfoResult>>(cancellationToken))?.Value ??
               new AccountInfoResult();
    }

    public async Task<AccountConfigResult> AccountConfig(
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

        return (await httpResponseMessage.Deserialize<ResultDto<AccountConfigResult>>(cancellationToken))?.Value ??
               new AccountConfigResult();
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