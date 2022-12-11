namespace Kavenegar.Core.Dto.Message;

public class SendMessageInfo
{
    public SendMessageInfo(
        MessageInfo messageInfo,
        string receptor)
    {
        MessageInfo = messageInfo;
        Receptor = receptor;
    }

    public MessageInfo MessageInfo { get; set; }
    public string Receptor { get; set; }
    public string LocalMessageId { get; set; } = null!;
}