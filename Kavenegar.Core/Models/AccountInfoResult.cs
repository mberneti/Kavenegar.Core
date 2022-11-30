using System;
using Kavenegar.Core.Utils;

namespace Kavenegar.Core.Models;

public class AccountInfoResult
{
    public long RemainCredit { get; set; }
    public long Expiredate { get; set; }

    public DateTime GregorianExpiredate => DateHelper.UnixTimestampToDateTime(Expiredate);

    public string Type { get; set; }
}