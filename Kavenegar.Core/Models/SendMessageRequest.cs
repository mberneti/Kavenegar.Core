using Kavenegar.Core.Models.Enums;

namespace Kavenegar.Core;

public class  SendMessageRequest
{
    public string Sender { get; set; } = null!;
    public string Receptor { get; set; } = null!;
    public string Message { get; set; } = null!;
    public MessageType? Type { get; set; }
    public string? LocalMessageId { get; set; }
}