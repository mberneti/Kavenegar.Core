using System.Text;
using System.Text.Json;

namespace Shared.Infrastructure;

public static class JsonUtility
{
    public static async Task<string> Serialize<T>(
        T obj,
        CancellationToken cancellationToken = default)
    {
        var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(
            stream,
            obj,
            cancellationToken: cancellationToken);
        var sr = new StreamReader(stream, Encoding.UTF8);
        return await sr.ReadToEndAsync();
    }

    public static async Task<T?> Deserialize<T>(
        Stream json,
        CancellationToken cancellationToken = default)
    {
        return await JsonSerializer.DeserializeAsync<T>(json, cancellationToken: cancellationToken);
    }
}