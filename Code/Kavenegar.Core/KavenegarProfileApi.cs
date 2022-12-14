using Kavenegar.Core.Dto.Result;
using Kavenegar.Core.Enums;
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
    
    public KavenegarProfileApi(
        string apiKey) : base(new HttpClientHelper(new HttpClient()), apiKey)
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
        IEnumerable<string> messageIds,
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
        IEnumerable<string> messageIds,
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
        IEnumerable<string> messageIds,
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
        DateTime? endDate = null,
        string? sender = null,
        CancellationToken cancellationToken = default)
    {
        var error = ValidateDatePeriod(startDate, endDate);
        if (error.IsNotNullOrWhiteSpace()) throw new ArgumentException(error);

        var queryParams = new Dictionary<string, object?>
        {
            {
                "startdate", startDate.ToUnixTimestamp()
            }
        };

        if (endDate.HasValue) queryParams.Add("enddate", endDate.Value.ToUnixTimestamp());
        if (sender.IsNotNullOrWhiteSpace()) queryParams.Add("sender", sender);

        return await RequestSender<List<SendResultDto>>(
            "sms/selectoutbox.json",
            null,
            queryParams,
            cancellationToken);
    }

    public async Task<List<SendResultDto>?> LatestOutbox(
        int? pageSize = null,
        string? sender = null,
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

        if (sender.IsNotNullOrWhiteSpace()) queryParams.Add("sender", sender);

        return await RequestSender<List<SendResultDto>>(
            "sms/latestoutbox.json",
            null,
            queryParams,
            cancellationToken);
    }

    public async Task<CountOutboxDto?> CountOutbox(
        DateTime startDate,
        DateTime? endDate = null,
        MessageStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var error = ValidateDatePeriod(startDate, endDate);
        if (error.IsNotNullOrWhiteSpace()) throw new ArgumentException(error);

        var queryParams = new Dictionary<string, object?>
        {
            {
                "startdate", startDate.ToUnixTimestamp()
            }
        };

        if (endDate.HasValue) queryParams.Add("enddate", endDate.Value.ToUnixTimestamp());
        if (status.HasValue) queryParams.Add("status", (int)status);

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
        IEnumerable<string> ids,
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
        DateTime? endDate = null,
        string? lineNumber = null,
        bool? isRead = null,
        CancellationToken cancellationToken = default)
    {
        var error = ValidateDatePeriod(startDate, endDate);
        if (error.IsNotNullOrWhiteSpace()) throw new ArgumentException(error);

        var queryParams = new Dictionary<string, object?>
        {
            {
                "startdate", startDate.ToUnixTimestamp()
            }
        };

        if (endDate.HasValue) queryParams.Add("enddate", endDate.Value.ToUnixTimestamp());
        if (lineNumber.IsNotNullOrWhiteSpace()) queryParams.Add("linenumber", lineNumber);
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
        string? apiLogs = null,
        string? dailyReport = null,
        string? debugMode = null,
        string? defaultSender = null,
        int? minCreditAlarm = null,
        string? resendFailed = null,
        CancellationToken cancellationToken = default)
    {
        if (apiLogs.IsNullOrWhiteSpace() &&
            dailyReport.IsNullOrWhiteSpace() &&
            debugMode.IsNullOrWhiteSpace() &&
            defaultSender.IsNullOrWhiteSpace() &&
            !minCreditAlarm.HasValue &&
            resendFailed.IsNullOrWhiteSpace())
            throw new ArgumentException("حداقل یکی از مقادیر باید وارد شده باشند.");

        var queryParams = new Dictionary<string, object?>();

        if (apiLogs.IsNotNullOrWhiteSpace()) queryParams.Add("apilogs", apiLogs);
        if (dailyReport.IsNotNullOrWhiteSpace()) queryParams.Add("dailyreport", dailyReport);
        if (debugMode.IsNotNullOrWhiteSpace()) queryParams.Add("debugmode", debugMode);
        if (defaultSender.IsNotNullOrWhiteSpace()) queryParams.Add("defaultsender", defaultSender);
        if (minCreditAlarm.HasValue) queryParams.Add("mincreditalarm", minCreditAlarm);
        if (resendFailed.IsNotNullOrWhiteSpace()) queryParams.Add("resendfailed", resendFailed);

        return await RequestSender<AccountConfigDto>(
            "account/config.json",
            null,
            queryParams,
            cancellationToken);
    }

    private string ValidateDatePeriod(
        DateTime startDate,
        DateTime? endDate = null)
    {
        if (startDate < DateTime.Now.AddDays(-60)) return "تاریخ شروع startDate حداکثر باید تا 60 روز قبل باشد.";

        if (endDate <= startDate) return "تاریخ پایان باید از تاریخ شروع بزرگتر باشد.";

        if (endDate.HasValue &&
            (endDate - startDate).Value.TotalDays > 1)
            return "حداکثر فاصله زمانی بین متغیر startDate تا متغیر endDate برابر با 1 روز می باشد.";

        return string.Empty;
    }
}