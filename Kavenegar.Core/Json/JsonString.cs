namespace Kavenegar.Core.Json;

public class JsonString : JsonObject
{
    public JsonString(
        string text)
    {
        Text = text;
    }

    public string Text { get; set; }

    public JsonObject UpCast()
    {
        JsonObject objectJ = this;
        return objectJ;
    }
}