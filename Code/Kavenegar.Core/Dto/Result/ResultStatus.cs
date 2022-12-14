using System.Text.Json.Serialization;

namespace Kavenegar.Core.Dto.Result;

internal class ResultStatus
{
    [JsonPropertyName("status")]
    public int Status { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;
}