using Kavenegar.Core.Models.Enums;

namespace Kavenegar.Core.Models;

public class StatusResult
{
    public long Messageid { get; set; }
    public MessageStatus Status { get; set; }
    public string Statustext { get; set; }
}