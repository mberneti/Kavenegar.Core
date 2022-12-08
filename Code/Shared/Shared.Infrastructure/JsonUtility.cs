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
        Stream json,
        CancellationToken cancellationToken = default)
    {
        return await JsonSerializer.DeserializeAsync<T>(json, cancellationToken: cancellationToken);
    }

    public static async Task<T?> Deserialize<T>(
        this HttpResponseMessage httpResponseMessage,
        CancellationToken cancellationToken = default)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream, Encoding.UTF8);
        await streamWriter.WriteAsync(content);
        return await Deserialize<T>(stream, cancellationToken);
    }
}