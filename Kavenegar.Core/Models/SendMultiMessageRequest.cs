using System;
using System.Collections.Generic;

namespace Kavenegar.Core.Models;

public class SendMultiMessageRequest
{
    public Dictionary<string, MessageInfo> Messages { get; set; } = null!;
    public DateTime Date { get; set; }
    public bool Hide { get; set; }
}