using System.Text.Json.Serialization;

namespace Kavenegar.Core.Dto.Result;

internal class ResultDto<T>
{
    [JsonPropertyName("return")]
    public ResultStatus Result { get; set; } = null!;

    [JsonPropertyName("entries")]
    public T Value { get; set; } = default!;
}