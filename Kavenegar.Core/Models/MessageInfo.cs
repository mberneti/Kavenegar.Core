using Kavenegar.Core.Models.Enums;

namespace Kavenegar.Core.Models;

public class MessageInfo
{
    public string Sender { get; set; } = null!;
    public string Message { get; set; } = null!;
    public MessageType Type { get; set; }
    public string LocalMessageId { get; set; } = null!;
}