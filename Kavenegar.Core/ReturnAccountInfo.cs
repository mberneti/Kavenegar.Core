using Kavenegar.Core.Models;

namespace Kavenegar.Core;

internal class ReturnAccountInfo
{
    public Result result { get; set; }
    public AccountInfoResult entries { get; set; }
}