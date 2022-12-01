using System.Collections.Generic;
using Kavenegar.Core.Models;

namespace Kavenegar.Core;

internal class ReturnCountOutbox
{
    public Result result { get; set; }
    public List<CountOutboxResult> entries { get; set; }
}