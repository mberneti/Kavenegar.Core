using System.Collections.Generic;
using Kavenegar.Core.Models;

namespace Kavenegar.Core;

internal class ReturnCountPostalCode
{
    public Result result { get; set; }
    public List<CountPostalCodeResult> entries { get; set; }
}