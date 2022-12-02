namespace Kavenegar.Core.Dto.Message;

public class SendMessageInfo
{
    public MessageInfo MessageInfo { get; set; } = null!;
    public string Receptor { get; set; } = null!;
    public string LocalMessageId { get; set; } = null!;
}