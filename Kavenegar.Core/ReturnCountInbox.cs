using System.Collections.Generic;
using Kavenegar.Core.Models;

namespace Kavenegar.Core;

internal class ReturnCountInbox
{
    public Result result { get; set; }
    public List<CountInboxResult> entries { get; set; }
}