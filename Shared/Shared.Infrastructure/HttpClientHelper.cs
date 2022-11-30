namespace Shared.Infrastructure;

public class HttpClientHelper
{
    private readonly HttpClient _httpClient = new();

    public async Task<HttpResponseMessage> PostAsync(
        string requestUri,
        object body,
        CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAsync(
            requestUri,
            await SerializeBody(body, cancellationToken),
            cancellationToken);
    }

    private async Task<StringContent> SerializeBody(
        object obj,
        CancellationToken cancellationToken)
    {
        return new StringContent(await JsonUtility.Serialize(obj, cancellationToken));
    }
}