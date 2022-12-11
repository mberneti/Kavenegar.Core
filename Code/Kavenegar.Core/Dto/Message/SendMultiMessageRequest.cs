namespace Kavenegar.Core.Dto.Message;

public class SendMultiMessageRequest
{
    public SendMultiMessageRequest(
        IEnumerable<SendMessageInfo> sendMessageInfos)
    {
        SendMessageInfos = sendMessageInfos;
    }

    public IEnumerable<SendMessageInfo> SendMessageInfos { get; set; }
    public DateTime Date { get; set; }
    public bool Hide { get; set; }
}