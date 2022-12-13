namespace Kavenegar.Core.Dto.Result;

public class CountOutboxDto : CountInboxDto
{
    public long SumPart { get; set; }
    public long Cost { get; set; }
}