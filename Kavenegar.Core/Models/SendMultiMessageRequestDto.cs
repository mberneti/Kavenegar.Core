using System.ComponentModel.DataAnnotations.Schema;

namespace Kavenegar.Core.Models;

internal class SendMultiMessageRequestDto
{
    [Column("receptor", Order = 1)]
    public string Receptors { get; set; } = null!;

    [Column("sender", Order = 2)]
    public string? Sender { get; set; }

    [Column("message", Order = 3)]
    public string Message { get; set; } = null!;

    [Column("date", Order = 4)]
    public long Date { get; set; }

    [Column("type", Order = 5)]
    public int Type { get; set; }

    [Column("localmessageids", Order = 6)]
    public string LocalMessageId { get; set; } = null!;

    [Column("hide", Order = 7)]
    public int Hide { get; set; }
}