namespace Shared.Infrastructure;

public class HttpClientHelper : IHttpClientHelper
{
    private readonly HttpClient _httpClient;

    public HttpClientHelper(
        HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string BaseAddress { get; set; } = "";

    public async Task<HttpResponseMessage> PostAsync(
        string requestUri,
        object? body = null,
        Dictionary<string, object?>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        var address = queryParams == null ?
            $"{BaseAddress}/{requestUri}" :
            QueryParamHelper.AddQueryParamToUri($"{BaseAddress}/{requestUri}", queryParams);

        return await _httpClient.PostAsync(
            address,
            body == null ? null : new StringContent(body.Serialize()),
            cancellationToken);
    }
}