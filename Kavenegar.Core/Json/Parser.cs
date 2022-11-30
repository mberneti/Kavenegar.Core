using System;
using System.Collections.Generic;

namespace Kavenegar.Core.Json;

/// <summary>
///     Contains the façade-style class to perform Json operations
/// </summary>
public class Parser
{
    private const char ObjectBegin = '{';
    private const char ObjectEnd = '}';
    private const char ArrayBegin = '[';
    private const char ArrayEnd = ']';
    private const char DoubleQuotes = '"';
    private const char DoublePoint = ':';
    private const char Comma = ',';
    private const char BackSlash = '\u005C';
    private const string NullValue = "null";
    private const string TrueValue = "true";
    private const string FalseValue = "false";

    public JsonObject DocumentJson;

    /// <summary>
    ///     Deserialize a JSON document. This method does not perform a syntax checking so It assumes a valid Json input
    /// </summary>
    /// <param name='json'>
    ///     A string which contains a valid Json array or object
    /// </param>
    public JsonObject Parse(
        string json)
    {
        if (json[0] == ArrayBegin)
        {
            json = json.Substring(1, json.Length - 2);
            var arrayJson = SerializeArray(json);
            JsonObject o = arrayJson;
            return o;
        }

        if (json[0] == ObjectBegin) return SerializeObject(json);

        return null;
    }

    /// <summary>
    ///     This method performs deserialization of an object(except array)
    /// </summary>
    /// <returns>
    ///     JsonObject object as a deserialized JSON object
    /// </returns>
    /// <param name='json'>
    ///     A string which contains a valid Json object
    /// </param>
    private JsonObject SerializeObject(
        string json)
    {
        json = json.Replace(@"\", "");
        var document = new JsonObject();
        var n = 1;
        var lengthJson = json.Length;
        var keyString = "";

        while (n <= lengthJson - 1)
            if (json[n] == DoubleQuotes &&
                json[n - 1] != DoublePoint)
            {
                var secondDoubleQuotes = FindNextQuote(json, n + 1);
                keyString = json.Substring(n + 1, secondDoubleQuotes - (n + 1));
                n = secondDoubleQuotes + 1;
            }
            else if (json[n] == DoubleQuotes &&
                     json[n - 1] == DoublePoint)
            {
                if (json[n + 1] != DoubleQuotes)
                {
                    var secondDoublesQuotes = FindNextQuote(json, n + 1);
                    var text = json.Substring(n + 1, secondDoublesQuotes - (n + 1));
                    var stringValue = new JsonString(text);
                    JsonObject o = stringValue;
                    document.AddJsonValue(keyString, o);
                    n = secondDoublesQuotes + 1;
                }
                else
                {
                    JsonObject o = new JsonString("");
                    document.AddJsonValue(keyString, o);
                }
            }
            else if (json[n] == '-' ||
                     json[n] == '0' ||
                     json[n] == '1' ||
                     json[n] == '2' ||
                     json[n] == '3' ||
                     json[n] == '4' ||
                     json[n] == '5' ||
                     json[n] == '6' ||
                     json[n] == '7' ||
                     json[n] == '8' ||
                     json[n] == '9')
            {
                char[] arrayEndings =
                {
                    ObjectEnd,
                    Comma
                };
                var nextComma = json.IndexOfAny(arrayEndings, n);
                var stringNumber = json.Substring(n, nextComma - n);
                var valueNumber = Convert.ToDouble(stringNumber);
                var floatNumber = (float)valueNumber;
                var number = new JsonNumber(floatNumber);
                JsonObject o = number;
                document.AddJsonValue(keyString, o);
                n = nextComma + 1;
            }
            else if (json[n] == ArrayBegin)
            {
                if (json[n + 1] != ArrayEnd)
                {
                    var subJson = json.Substring(n, json.Length - n);
                    var arrayClose = CloseBracketArray(subJson);
                    var arrayUnknown = json.Substring(n + 1, arrayClose - 2);
                    var arrayObjects = SerializeArray(arrayUnknown);
                    JsonObject o = arrayObjects;
                    document.AddJsonValue(keyString, o);
                    n = n + arrayClose;
                }
                else
                {
                    if (!string.IsNullOrEmpty(keyString))
                    {
                        var arrayTempEmpty = new JsonArray
                        {
                            Array = null
                        };
                        JsonObject emptyArray = arrayTempEmpty;
                        document.AddJsonValue(keyString, emptyArray);
                        keyString = "";
                    }
                    else
                    {
                        n++;
                    }
                }
            }
            else if (json[n] == ObjectBegin)
            {
                if (json[n + 1] != ObjectEnd)
                {
                    var subJson = json.Substring(n, json.Length - n);
                    var objectClose = CloseBracketObject(subJson);
                    var objectUnknown = json.Substring(n, objectClose);
                    var o = SerializeObject(objectUnknown);
                    document.AddJsonValue(keyString, o);
                    n = n + objectClose + 1;
                }
                else
                {
                    var o = new JsonObject
                    {
                        Values = null
                    };
                    document.AddJsonValue(keyString, o);
                }
            }
            else if (string.Compare(
                         SafeSubString(
                             json,
                             n,
                             4),
                         NullValue,
                         StringComparison.Ordinal) ==
                     0)
            {
                JsonObject o = new JsonNullable();
                document.AddJsonValue(keyString, o);
                n = n + 5;
            }
            else if (string.Compare(
                         SafeSubString(
                             json,
                             n,
                             4),
                         TrueValue,
                         StringComparison.Ordinal) ==
                     0)
            {
                JsonObject o = new JsonBoolean(true);
                document.AddJsonValue(keyString, o);
                n = n + 5;
            }
            else if (string.Compare(
                         SafeSubString(
                             json,
                             n,
                             5),
                         FalseValue,
                         StringComparison.Ordinal) ==
                     0)
            {
                JsonObject o = new JsonBoolean(false);
                document.AddJsonValue(keyString, o);
                n = n + 6;
            }
            else
            {
                n++;
            }

        return document;
    }

    /// <summary>
    ///     Search where is the ending of an object
    /// </summary>
    /// <returns>
    ///     the index of the '}' which closes an object
    /// </returns>
    /// <param name='json'>
    ///     A valid json string ({........)
    /// </param>
    private int CloseBracketObject(
        string json)
    {
        var countObjectBegin = 0;
        var countObjectEnd = 0;
        var n = 0;

        do
        {
            if (json[n] == ObjectBegin)
                countObjectBegin++;
            else if (json[n] == ObjectEnd) countObjectEnd++;

            n++;
        }
        while (countObjectBegin != countObjectEnd);

        return n;
    }

    /// <summary>
    ///     Search where is the ending of an array
    /// </summary>
    /// <returns>
    ///     he index of the ']' which closes an object
    /// </returns>
    /// <param name='json'>
    ///     A valid Json string ([.....)
    /// </param>
    private int CloseBracketArray(
        string json)
    {
        var countArrayBegin = 0;
        var countArrayEnd = 0;
        var n = 0;

        do
        {
            if (json[n] == ArrayBegin)
                countArrayBegin++;
            else if (json[n] == ArrayEnd) countArrayEnd++;

            n++;
        }
        while (countArrayBegin != countArrayEnd);

        return n;
    }

    /// <summary>
    ///     Deserialize a Json Array into an object JsonArray
    /// </summary>
    /// <returns>
    ///     JsonArray object as a deserialized JSON array
    /// </returns>
    /// <param name='array'>
    ///     valid JSON array except the brackets
    /// </param>
    private JsonArray SerializeArray(
        string array)
    {
        var arrayObject = new JsonArray();
        var elements = SplitElements(array);

        foreach (var item in elements)
            if (item[0] == DoubleQuotes)
            {
                var withoutQuotes = item.Trim(DoubleQuotes);
                JsonObject o = new JsonString(withoutQuotes);
                arrayObject.AddElementToArray(o);
            }
            else if (item[0] == ObjectBegin)
            {
                var o = SerializeObject(item);
                arrayObject.AddElementToArray(o);
            }
            else if (item[0] == ArrayBegin)
            {
                var itemArray = item.Substring(1, item.Length - 2);
                var secondaryArray = SerializeArray(itemArray);
                JsonObject o = secondaryArray;
                arrayObject.AddElementToArray(o);
            }
            else if (item[0] == '-' ||
                     item[0] == '0' ||
                     item[0] == '1' ||
                     item[0] == '2' ||
                     item[0] == '3' ||
                     item[0] == '4' ||
                     item[0] == '5' ||
                     item[0] == '6' ||
                     item[0] == '7' ||
                     item[0] == '8' ||
                     item[0] == '9')
            {
                var doubleValue = Convert.ToDouble(item);
                var floatValue = (float)doubleValue;
                JsonObject o = new JsonNumber(floatValue);
                arrayObject.AddElementToArray(o);
            }
            else if (string.Compare(
                         SafeSubString(
                             item,
                             0,
                             4),
                         TrueValue,
                         StringComparison.Ordinal) ==
                     0)
            {
                JsonObject o = new JsonBoolean(true);
                arrayObject.AddElementToArray(o);
            }
            else if (string.Compare(
                         SafeSubString(
                             item,
                             0,
                             5),
                         FalseValue,
                         StringComparison.Ordinal) ==
                     0)
            {
                JsonObject o = new JsonBoolean(false);
                arrayObject.AddElementToArray(o);
            }
            else if (string.Compare(
                         SafeSubString(
                             item,
                             0,
                             4),
                         NullValue,
                         StringComparison.Ordinal) ==
                     0)
            {
                JsonObject o = new JsonNullable();
                arrayObject.AddElementToArray(o);
            }

        return arrayObject;
    }

    /// <summary>
    ///     Just a safe subString operation
    /// </summary>
    /// <returns>
    ///     A subString of the string input parameter text
    /// </returns>
    /// <param name='text'>
    ///     A string
    /// </param>
    /// <param name='start'>
    ///     index of starting
    /// </param>
    /// <param name='length'>
    ///     Length of the subString
    /// </param>
    private string SafeSubString(
        string text,
        int start,
        int length)
    {
        var safeString = start + length < text.Length ?
            text.Substring(start, length) :
            text.Substring(start, text.Length - start);

        return safeString;
    }

    /// <summary>
    ///     Finds the next '"' to close a String field
    /// </summary>
    /// <returns>
    ///     The next ' " '
    /// </returns>
    /// <param name='text'>
    ///     A valid JSON string
    /// </param>
    /// <param name='index'>
    ///     Index of starting
    /// </param>
    private int FindNextQuote(
        string text,
        int index)
    {
        var nextQuote = text.IndexOf(DoubleQuotes, index);
        while (text[nextQuote - 1] == BackSlash) nextQuote = text.IndexOf(DoubleQuotes, nextQuote + 1);

        return nextQuote;
    }

    /// <summary>
    ///     Splits the elements of an Array
    /// </summary>
    /// <returns>
    ///     The elements in an array of Strings
    /// </returns>
    /// <param name='arrayText'>
    /// </param>
    private string[] SplitElements(
        string arrayText)
    {
        var n = 0;
        var doubleQuotesCounter = 0;
        var objectBeginCounter = 0;
        var objectEndCounter = 0;
        var arrayBeginCounter = 0;
        var arrayEndCounter = 0;
        var previousCommaIndex = 0;
        var oneElement = true;
        var textSplit = new List<string>();

        while (n <= arrayText.Length - 1)
            if (arrayText[n] == DoubleQuotes &&
                arrayText[n - 1] != BackSlash)
            {
                doubleQuotesCounter++;
                n++;
            }
            else if (arrayText[n] == ObjectBegin)
            {
                objectBeginCounter++;
                n++;
            }
            else if (arrayText[n] == ObjectEnd)
            {
                objectEndCounter++;
                n++;
            }
            else if (arrayText[n] == ArrayBegin)
            {
                arrayBeginCounter++;
                n++;
            }
            else if (arrayText[n] == ArrayEnd)
            {
                arrayEndCounter++;
                n++;
            }
            else if (arrayText[n] == Comma &&
                     doubleQuotesCounter % 2 == 0 &&
                     objectBeginCounter == objectEndCounter &&
                     arrayBeginCounter == arrayEndCounter)
            {
                textSplit.Add(arrayText.Substring(previousCommaIndex, n - previousCommaIndex));
                previousCommaIndex = n + 1;
                n++;
                oneElement = false;
            }
            else
            {
                n++;
            }

        textSplit.Add(
            oneElement ? arrayText : arrayText.Substring(previousCommaIndex, arrayText.Length - previousCommaIndex));

        var textSplitArray = textSplit.ToArray();
        return textSplitArray;
    }
}