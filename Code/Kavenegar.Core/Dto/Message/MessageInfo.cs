using Kavenegar.Core.Enums;

namespace Kavenegar.Core.Dto.Message;

public class MessageInfo
{
    public MessageInfo(
        string message)
    {
        Message = message;
    }

    public string? Sender { get; set; }
    public string Message { get; set; }
    public MessageType Type { get; set; } = MessageType.Flash;
}