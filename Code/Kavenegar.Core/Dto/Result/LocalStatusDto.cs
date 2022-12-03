using Kavenegar.Core.Enums;

namespace Kavenegar.Core.Dto.Result;

public class LocalStatusDto
{
    public long MessageId { get; set; }
    public long LocalId { get; set; }
    public MessageStatus Status { get; set; }
    public string StatusText { get; set; } = null!;
}