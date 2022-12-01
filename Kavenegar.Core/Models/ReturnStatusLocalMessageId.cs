using System.Collections.Generic;
using Kavenegar.Core.Models;

namespace Kavenegar.Core;

internal class ReturnStatusLocalMessageId
{
    public Result result { get; set; }
    public List<StatusLocalMessageIdResult> entries { get; set; }
}