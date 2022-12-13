namespace Kavenegar.Core.Dto.Message;

public class SendSingleMessageRequest
{
    public SendSingleMessageRequest(
        MessageInfo messageInfo,
        Dictionary<string, string?> receptorLocalMessageIds)
    {
        MessageInfo = messageInfo;
        ReceptorLocalMessageIds = receptorLocalMessageIds;
    }

    public MessageInfo MessageInfo { get; set; }

    /// <summary>
    ///     Key is receptor
    ///     Value is local message id
    /// </summary>
    public Dictionary<string, string?> ReceptorLocalMessageIds { get; set; }

    public DateTime? Date { get; set; }
    public bool Hide { get; set; }
}