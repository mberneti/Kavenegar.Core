using System.Collections.Generic;

namespace Kavenegar.Core.Json;

/// <summary>
///     JsonObject is the base class.
///     JsonString,JsonNumber,JsonBoolean,JsonNullable and JsonArray inherits from JsonObject.
///     A JsonArray object may contain objects of the base class
/// </summary>
public class JsonObject
{
    public Dictionary<string, JsonObject> Values;

    public JsonObject()
    {
        Values = new Dictionary<string, JsonObject>();
    }

    public void AddJsonValue(
        string textTag,
        JsonObject newObject)
    {
        if (!Values.ContainsKey(textTag)) Values.Add(textTag, newObject);
    }

    public JsonObject GetObject(
        string key)
    {
        var current = Values[key];
        return current;
    }

    public int ElementsOfDictionary()
    {
        return Values.Count;
    }


    public bool IsJsonString()
    {
        if (this is JsonString) return true;
        return false;
    }

    public bool IsJsonNumber()
    {
        if (this is JsonNumber) return true;
        return false;
    }

    public bool IsJsonBoolean()
    {
        if (this is JsonBoolean) return true;
        return false;
    }

    public bool IsJsonNullable()
    {
        if (this is JsonNullable) return true;
        return false;
    }

    public bool IsJsonArray()
    {
        if (this is JsonArray) return true;
        return false;
    }

    public JsonString GetAsString()
    {
        return (JsonString)this;
    }

    public JsonNumber GetAsNumber()
    {
        return (JsonNumber)this;
    }

    public JsonArray GetAsArray()
    {
        return (JsonArray)this;
    }
}