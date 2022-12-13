using System;
using NUnit.Framework;
using Shared.Infrastructure;

namespace Test.Shared.Infrastructure;

public class DateHelperTests
{
    [Test]
    public void ToDateTime_WhenCalled_EqualsToConvertedDateTime()
    {
        const long unixTimeStamp = 1577836800L;

        var result = unixTimeStamp.ToDateTime();

        var expectedDateTime = new DateTime(
            2020,
            1,
            1);

        Assert.That(result, Is.EqualTo(expectedDateTime));
    }

    [Test]
    public void ToUnixTimestamp_DateBeforeMinDateTime_ThrowsArgumentException()
    {
        var dt = new DateTime(
            1969,
            12,
            1);

        Assert.That(() => dt.ToUnixTimestamp(), Throws.ArgumentException);
    }

    [Test]
    public void ToUnixTimestamp_DateAfterMinDateTime_ReturnsUnixTimestamp()
    {
        var dt = new DateTime(
            2020,
            1,
            1);

        var unixTimestamp = dt.ToUnixTimestamp();

        Assert.That(unixTimestamp, Is.EqualTo(1577836800));
    }
}