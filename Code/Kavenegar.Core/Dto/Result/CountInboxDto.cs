using Shared.Infrastructure;

namespace Kavenegar.Core.Dto.Result;

public class CountInboxDto
{
    public long StartDate { get; set; }
    public DateTime StartDateGregorian => StartDate.ToDateTime();
    public long EndDate { get; set; }
    public DateTime EndDateGregorian => EndDate.ToDateTime();

    public long SumCount { get; set; }
}