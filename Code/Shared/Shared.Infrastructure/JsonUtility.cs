using System.Text.Json;

namespace Shared.Infrastructure;

public static class JsonUtility
{
    public static string Serialize<T>(
        this T obj)
    {
        return JsonSerializer.Serialize(obj, obj!.GetType());
    }

    public static async Task<T?> Deserialize<T>(
        this HttpResponseMessage httpResponseMessage,
        CancellationToken cancellationToken = default)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(content);
    }
}