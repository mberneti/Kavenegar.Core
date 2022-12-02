namespace Kavenegar.Core.Dto.Message;

public class SendMultiMessageRequest
{
    public IEnumerable<SendMessageInfo> SendMessageInfos { get; set; } = null!;
    public DateTime Date { get; set; }
    public bool Hide { get; set; }
}