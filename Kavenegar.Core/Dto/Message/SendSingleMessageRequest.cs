namespace Kavenegar.Core.Dto.Message;

public class SendSingleMessageRequest
{
    public MessageInfo MessageInfo { get; set; } = null!;
    
    /// <summary>
    /// Key is receptor
    /// Value is local message id
    /// </summary>
    public Dictionary<string,string> ReceptorLocalMessageIds { get; set; } = null!;
    public DateTime? Date { get; set; }
    public bool Hide { get; set; }
}