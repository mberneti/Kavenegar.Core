using Kavenegar.Core.Enums;

namespace Kavenegar.Core.Dto.Message;

public class MessageInfo
{
    public string Sender { get; set; } = null!;
    public string Message { get; set; } = null!;
    public MessageType Type { get; set; } = MessageType.Flash;
}