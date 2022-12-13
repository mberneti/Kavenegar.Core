using Shared.Infrastructure;

namespace Kavenegar.Core.Dto.Result;

public class AccountInfoDto
{
    public long RemainCredit { get; set; }
    public long ExpireDate { get; set; }
    public DateTime GregorianExpireDate => ExpireDate.ToDateTime();
    public string Type { get; set; } = null!;
}