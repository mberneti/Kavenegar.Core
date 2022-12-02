using System;
using System.Collections.Generic;

namespace Kavenegar.Core.Models;

public class SendSingleMessageRequest
{
    public MessageInfo MessageInfo { get; set; } = null!;
    public IEnumerable<string> Receptors { get; set; } = null!;
    public DateTime Date { get; set; }
    public bool Hide { get; set; }
}