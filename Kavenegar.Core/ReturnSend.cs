using System.Collections.Generic;
using Kavenegar.Core.Models;

namespace Kavenegar.Core;

internal class ReturnSend
{
    public Result Return { get; set; }
    public List<SendResult> entries { get; set; }
}