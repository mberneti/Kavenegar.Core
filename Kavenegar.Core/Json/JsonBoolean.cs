namespace Kavenegar.Core.Json;

public class JsonBoolean : JsonObject
{
    public JsonBoolean(
        bool booleanValue)
    {
        BooleanValue = booleanValue;
    }

    public bool BooleanValue { get; set; }

    public JsonObject UpCast()
    {
        JsonObject objectJ = this;
        return objectJ;
    }
}