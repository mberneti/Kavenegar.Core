using System.Text.Json.Serialization;
using Kavenegar.Core.Enums;
using Shared.Infrastructure;

namespace Kavenegar.Core.Dto.Result;

public class SendResultDto
{
    public long MessageId { get; set; }
    public string Message { get; set; } = null!;

    [JsonPropertyName("Status")]
    public int StatusNumber { get; set; }

    public MessageStatus Status => StatusCaster(StatusNumber);
    public string StatusText { get; set; } = null!;
    public string Sender { get; set; } = null!;
    public string Receptor { get; set; } = null!;

    [JsonPropertyName("Date")]
    public long UnixDate { get; set; }

    public DateTime DateTime => UnixDate.ToDateTime();
    public int Cost { get; set; }

    private MessageStatus StatusCaster(
        int status)
    {
        if (Enum.IsDefined(typeof(MessageStatus), StatusNumber)) return (MessageStatus)status;
        return status == 5 ? MessageStatus.SentToCenter : MessageStatus.Unknown;
    }
}