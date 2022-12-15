using Kavenegar.Core;
using Kavenegar.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTest.Controllers;

public class KavenegarProfileController : BaseAPiController
{
    private readonly IKavenegarProfileApi _kavenegarProfileApi;

    public KavenegarProfileController(
        IKavenegarProfileApi kavenegarProfileApi)
    {
        _kavenegarProfileApi = kavenegarProfileApi;
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(
        IEnumerable<string> ids)
    {
        return HandleValue(await _kavenegarProfileApi.Cancel(ids));
    }

    [HttpGet]
    public async Task<IActionResult> Receive(
        string line,
        bool isRead)
    {
        return HandleValue(await _kavenegarProfileApi.Receive(line, isRead));
    }

    [HttpGet]
    public async Task<IActionResult> Select(
        [FromQuery] IEnumerable<string> messageIds)
    {
        return HandleValue(await _kavenegarProfileApi.Select(messageIds));
    }

    [HttpGet]
    public async Task<IActionResult> Status(
        [FromQuery] IEnumerable<string> messageIds)
    {
        return HandleValue(await _kavenegarProfileApi.Status(messageIds));
    }

    [HttpPost]
    public async Task<IActionResult> AccountConfig(
        string? apiLogs = null,
        string? dailyReport = null,
        string? debugMode = null,
        string? defaultSender = null,
        int? minCreditAlarm = null,
        string? resendFailed = null)
    {
        return HandleValue(
            await _kavenegarProfileApi.AccountConfig(
                apiLogs,
                dailyReport,
                debugMode,
                defaultSender,
                minCreditAlarm,
                resendFailed));
    }

    [HttpGet]
    public async Task<IActionResult> AccountInfo()
    {
        return HandleValue(await _kavenegarProfileApi.AccountInfo());
    }

    [HttpGet]
    public async Task<IActionResult> CountInbox(
        DateTime startDate,
        DateTime? endDate,
        string? lineNumber,
        bool? isRead)
    {
        return HandleValue(
            await _kavenegarProfileApi.CountInbox(
                startDate,
                endDate,
                lineNumber,
                isRead));
    }

    [HttpGet]
    public async Task<IActionResult> CountOutbox(
        DateTime startDate,
        DateTime? endDate = null,
        MessageStatus? status = null)
    {
        return HandleValue(
            await _kavenegarProfileApi.CountOutbox(
                startDate,
                endDate,
                status));
    }

    [HttpGet]
    public async Task<IActionResult> LatestOutbox(
        int? pageSize = null,
        string? sender = null)
    {
        return HandleValue(await _kavenegarProfileApi.LatestOutbox(pageSize, sender));
    }

    [HttpGet]
    public async Task<IActionResult> SelectOutbox(
        DateTime startDate,
        DateTime? endDate = null,
        string? sender = null)
    {
        return HandleValue(
            await _kavenegarProfileApi.SelectOutbox(
                startDate,
                endDate,
                sender));
    }

    [HttpGet]
    public async Task<IActionResult> StatusLocalMessageId(
        [FromQuery] IEnumerable<string> messageIds)
    {
        return HandleValue(await _kavenegarProfileApi.StatusLocalMessageId(messageIds));
    }
}