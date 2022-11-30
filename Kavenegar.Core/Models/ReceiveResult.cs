using System;
using Kavenegar.Core.Utils;

namespace Kavenegar.Core.Models;

public class ReceiveResult
{
    public long Date { get; set; }

    public DateTime GregorianDate => DateHelper.UnixTimestampToDateTime(Date);

    public long MessageId { get; set; }

    public string Sender { get; set; }

    public string Message { get; set; }

    public string Receptor { get; set; }
}