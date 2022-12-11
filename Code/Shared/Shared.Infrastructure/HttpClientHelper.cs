namespace Shared.Infrastructure;

public class HttpClientHelper : IHttpClientHelper
{
    private readonly HttpClient _httpClient = new();

    public string BaseAddress { get; set; } = null!;

    public async Task<HttpResponseMessage> PostAsync(
        string requestUri,
        object? body = null,
        Dictionary<string, object?>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        var address = queryParams == null ?
            Path.Combine(BaseAddress, requestUri) :
            QueryParamHelper.AddQueryParamToUri(Path.Combine(BaseAddress, requestUri), queryParams);

        return await _httpClient.PostAsync(
            address,
            body == null ? null : SerializeBody(body, cancellationToken),
            cancellationToken);
    }

    private StringContent SerializeBody(
        object obj,
        CancellationToken cancellationToken)
    {
        return new StringContent(obj.Serialize(cancellationToken));
    }
}