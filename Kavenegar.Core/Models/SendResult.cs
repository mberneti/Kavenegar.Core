namespace Kavenegar.Core.Models;

public class SendResult
{
    public long MessageId { get; set; }
    public string Message { get; set; } = null!;
    public int Status { get; set; }
    public string StatusText { get; set; } = null!;
    public string Sender { get; set; } = null!;
    public string Receptor { get; set; } = null!;
    public long Date { get; set; }
    public int Cost { get; set; }
}