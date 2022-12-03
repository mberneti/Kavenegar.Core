namespace Shared.Infrastructure;

public static class DateHelper
{
    private static readonly DateTime MinUnixDateTime = new(
        1970,
        1,
        1,
        0,
        0,
        0,
        0,
        DateTimeKind.Local);

    public static DateTime ToDateTime(
        this long unixTimeStamp)
    {
        return MinUnixDateTime.AddSeconds(unixTimeStamp);
    }

    public static DateTime ToDateTime(
        this double unixTimeStamp)
    {
        return MinUnixDateTime.AddSeconds(unixTimeStamp);
    }

    public static double ToUnixTimestamp(
        this DateTime dt)
    {
        if (dt < MinUnixDateTime) throw new ArgumentException("dt cannot be before min unix date time");

        var unixTimeSpan = dt - MinUnixDateTime.ToLocalTime();
        return unixTimeSpan.TotalSeconds;
    }
}