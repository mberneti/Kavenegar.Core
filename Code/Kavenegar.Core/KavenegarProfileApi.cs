using Kavenegar.Core.Dto.Result;
using Shared.Infrastructure;

namespace Kavenegar.Core;

public class KavenegarProfileApi
    : BaseKavenegarApi,
        IKavenegarProfileApi
{
    public KavenegarProfileApi(
        IHttpClientHelper httpClientHelper,
        string apiKey) : base(httpClientHelper, apiKey)
    {
    }

    public async Task<StatusMessageDto?> Status(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        return (await Status(
            new List<string>
            {
                messageId
            },
            cancellationToken))?.FirstOrDefault()!;
    }

    public async Task<List<StatusMessageDto>?> Status(
        List<string> messageIds,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "messageid", string.Join(',', messageIds)
            }
        };

        return await RequestSender<List<StatusMessageDto>>(
            "sms/status.json",
            null,
            queryParams,
            cancellationToken);
    }

    public async Task<LocalStatusDto?> StatusLocalMessageId(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        return (await StatusLocalMessageId(
            new List<string>
            {
                messageId
            },
            cancellationToken))?.FirstOrDefault()!;
    }

    public async Task<List<LocalStatusDto>?> StatusLocalMessageId(
        List<string> messageIds,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "messageid", string.Join(',', messageIds)
            }
        };

        return await RequestSender<List<LocalStatusDto>>(
            "sms/statuslocalmessageid.json",
            null,
            queryParams,
            cancellationToken);
    }

    public async Task<SendResultDto?> Select(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        return (await Select(
            new List<string>
            {
                messageId
            },
            cancellationToken))?.FirstOrDefault()!;
    }

    public async Task<List<SendResultDto>?> Select(
        List<string> messageIds,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "messageid", string.Join(',', messageIds)
            }
        };

        return await RequestSender<List<SendResultDto>>(
            "sms/select.json",
            null,
            queryParams,
            cancellationToken);
    }

    public async Task<List<SendResultDto>?> SelectOutbox(
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

        return await RequestSender<List<SendResultDto>>(
            "sms/selectoutbox.json",
            null,
            queryParams,
            cancellationToken);
    }

    public async Task<List<SendResultDto>?> LatestOutbox(
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

        return await RequestSender<List<SendResultDto>>(
            "sms/latestoutbox.json",
            null,
            queryParams,
            cancellationToken);
    }

    public async Task<CountOutboxDto?> CountOutbox(
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

        return (await RequestSender<List<CountOutboxDto>>(
            "sms/countoutbox.json",
            null,
            queryParams,
            cancellationToken))?.FirstOrDefault();
    }

    public async Task<StatusMessageDto?> Cancel(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        return (await Cancel(
            new List<string>
            {
                messageId
            },
            cancellationToken))?.FirstOrDefault()!;
    }

    public async Task<List<StatusMessageDto>?> Cancel(
        List<string> ids,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            {
                "messageid", string.Join(',', ids)
            }
        };
        return await RequestSender<List<StatusMessageDto>>(
            "sms/cancel.json",
            null,
            queryParams,
            cancellationToken);
    }

    public async Task<List<ReceivedMessageDto>?> Receive(
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

        return await RequestSender<List<ReceivedMessageDto>>(
            "sms/receive.json",
            null,
            queryParams,
            cancellationToken);
    }

    public async Task<CountInboxDto?> CountInbox(
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

        return (await RequestSender<List<CountInboxDto>>(
            "sms/countinbox.json",
            null,
            queryParams,
            cancellationToken))?.FirstOrDefault();
    }

    public async Task<AccountInfoDto?> AccountInfo(
        CancellationToken cancellationToken = default)
    {
        return await RequestSender<AccountInfoDto>(
            "account/info.json",
            null,
            null,
            cancellationToken);
    }

    public async Task<AccountConfigDto?> AccountConfig(
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

        return await RequestSender<AccountConfigDto>(
            "account/config.json",
            null,
            queryParams,
            cancellationToken);
    }
}