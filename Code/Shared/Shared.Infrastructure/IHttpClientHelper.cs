namespace Shared.Infrastructure;

public interface IHttpClientHelper
{
    string BaseAddress { get; set; }

    Task<HttpResponseMessage> PostAsync(
        string requestUri,
        object? body = null,
        Dictionary<string, object?>? queryParams = null,
        CancellationToken cancellationToken = default);
}