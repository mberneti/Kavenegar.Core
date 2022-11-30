namespace Kavenegar.Core.Json;

public class JsonNumber : JsonObject
{
    public JsonNumber(
        float number)
    {
        Number = number;
    }

    public float Number { get; set; }

    public JsonObject UpCast()
    {
        JsonObject objectJ = this;
        return objectJ;
    }
}