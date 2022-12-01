using System.Collections.Generic;
using Kavenegar.Core.Models;

namespace Kavenegar.Core;

internal class ReturnReceive
{
    public Result result { get; set; }
    public List<ReceiveResult> entries { get; set; }
}