namespace Shared.Infrastructure;

public static class StringUtility
{
    public static bool IsNullOrWhiteSpace(
        this string? text)
    {
        return string.IsNullOrWhiteSpace(text);
    }

    public static bool IsNotNullOrWhiteSpace(
        this string? text)
    {
        return !text.IsNullOrWhiteSpace();
    }
}