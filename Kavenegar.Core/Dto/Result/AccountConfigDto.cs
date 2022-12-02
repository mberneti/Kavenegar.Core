namespace Kavenegar.Core.Dto.Result;

public class AccountConfigDto
{
    public string ApiLogs { get; set; } = null!;
    public string DailyReport { get; set; }= null!;
    public string DebugMode { get; set; }= null!;
    public string DefaultSender { get; set; }= null!;
    public string MinCreditAlarm { get; set; }= null!;
    public string ResendFailed { get; set; }= null!;
}