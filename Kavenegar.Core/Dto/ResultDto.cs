using Kavenegar.Core.Models;
using Newtonsoft.Json;

namespace Kavenegar.Core.Dto;

internal class ResultDto<T>
{
    [JsonProperty("return")]
    public Result Result { get; set; } = null!;

    [JsonProperty("entries")]
    public T Value { get; set; } = default!;
}