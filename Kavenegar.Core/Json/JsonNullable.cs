namespace Kavenegar.Core.Json;

public class JsonNullable : JsonObject
{
    public JsonNullable()
    {
        Nullable = "Null";
    }

    public string Nullable { get; set; }

    public JsonObject UpCast()
    {
        JsonObject objectJ = this;
        return objectJ;
    }
}