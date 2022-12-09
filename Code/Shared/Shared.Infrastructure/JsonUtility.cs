using System.Text;
using System.Text.Json;

namespace Shared.Infrastructure;

public static class JsonUtility
{
    public static async Task<string?> Serialize<T>(
        this T obj,
        CancellationToken cancellationToken = default)
    {
        var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(
            stream,
            obj,
            cancellationToken: cancellationToken);

        var streamReader = new StreamReader(stream, Encoding.UTF8);
        return await streamReader.ReadToEndAsync();
    }

    public static async Task<T?> Deserialize<T>(
        this HttpResponseMessage httpResponseMessage,
        CancellationToken cancellationToken = default)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(content);
    }
}