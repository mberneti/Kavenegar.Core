using System.Collections.Generic;
using Kavenegar.Core.Models;

namespace Kavenegar.Core;

internal class ReturnStatus
{
    public Result result { get; set; }
    public List<StatusResult> entries { get; set; }
}