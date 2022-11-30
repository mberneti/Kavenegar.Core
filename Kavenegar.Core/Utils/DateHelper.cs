using System;

namespace Kavenegar.Core.Utils;

public static class DateHelper
{
    private static DateTime _minUnixDateTime = new(
        1970,
        1,
        1,
        0,
        0,
        0,
        0,
        DateTimeKind.Local);

    public static DateTime UnixTimestampToDateTime(
        long unixTimeStamp)
    {
        return _minUnixDateTime.AddSeconds(unixTimeStamp);
    }

    public static DateTime UnixTimestampToDateTime(
        double unixTimeStamp)
    {
        return _minUnixDateTime.AddSeconds(unixTimeStamp);
    }

    public static double DateTimeToUnixTimestamp(
        DateTime dt)
    {
        if (dt < _minUnixDateTime) throw new ArgumentException("dt cannot be before min unix date time");

        var unixTimeSpan = dt - _minUnixDateTime.ToLocalTime();
        return unixTimeSpan.TotalSeconds;
    }
}