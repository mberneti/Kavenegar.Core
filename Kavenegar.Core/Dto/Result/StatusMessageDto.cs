using Kavenegar.Core.Enums;
using Kavenegar.Core.Models.Enums;

namespace Kavenegar.Core.Dto.Result;

public class StatusMessageDto
{
    public long Messageid { get; set; }
    public MessageStatus Status { get; set; }
    public string StatusText { get; set; } = null!;
}