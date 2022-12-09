using Kavenegar.Core.Dto.Result;
using Shared.Infrastructure;

namespace Kavenegar.Core;

public class BaseKavenegarApi
{
    private const string ApiAddress = "https://api.kavenegar.com/v1";
    private readonly IHttpClientHelper _httpClientHelper;

    protected BaseKavenegarApi(
        IHttpClientHelper httpClientHelper,
        string apiKey)
    {
        _httpClientHelper = httpClientHelper;
        _httpClientHelper.BaseAddress = Path.Combine(ApiAddress, apiKey);
    }

    protected async Task<T?> RequestSender<T>(
        string requestUri,
        object? body,
        Dictionary<string, object?>? queryParams,
        CancellationToken cancellationToken)
    {
        var httpResponseMessage = await _httpClientHelper.PostAsync(
            requestUri,
            body,
            queryParams,
            cancellationToken);

        var deserializedObj = await httpResponseMessage.Deserialize<ResultDto<T>>(cancellationToken);

        return deserializedObj == null ? default : deserializedObj.Value;
    }
}