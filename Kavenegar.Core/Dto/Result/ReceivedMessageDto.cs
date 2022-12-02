using Shared.Infrastructure;

namespace Kavenegar.Core.Dto.Result;

public class ReceivedMessageDto
{
    public long Date { get; set; }
    public DateTime GregorianDate => Date.ToDateTime();
    public long MessageId { get; set; }
    public string Sender { get; set; }= null!;
    public string Message { get; set; }= null!;
    public string Receptor { get; set; }= null!;
}