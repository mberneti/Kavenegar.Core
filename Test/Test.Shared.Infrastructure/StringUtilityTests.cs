using NUnit.Framework;
using Shared.Infrastructure;

namespace Test.Shared.Infrastructure;

[TestFixture]
public class StringUtilityTests
{
    [Test]
    public void IsNullOrWhiteSpace_EmptyString_ReturnsTrue()
    {
        var result = string.Empty.IsNullOrWhiteSpace();

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsNullOrWhiteSpace_StringWithZeroLength_ReturnsTrue()
    {
        var result = "".IsNullOrWhiteSpace();

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsNullOrWhiteSpace_NullString_ReturnsTrue()
    {
        string? s = null;

        var result = s.IsNullOrWhiteSpace();

        Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(" ")]
    [TestCase("  ")]
    [TestCase("   ")]
    public void IsNullOrWhiteSpace_WhiteSpaceString_ReturnsTrue(
        string s)
    {
        var result = s.IsNullOrWhiteSpace();

        Assert.That(result, Is.True);
    }

    [Test]
    [TestCase("a")]
    [TestCase("ab")]
    [TestCase("abc")]
    public void IsNullOrWhiteSpace_StringWithCharacter_ReturnsFalse(
        string s)
    {
        var result = s.IsNullOrWhiteSpace();

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsNotNullOrWhiteSpace_EmptyString_ReturnsTrue()
    {
        var result = string.Empty.IsNotNullOrWhiteSpace();

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsNotNullOrWhiteSpace_StringWithZeroLength_ReturnsTrue()
    {
        var result = "".IsNotNullOrWhiteSpace();

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsNotNullOrWhiteSpace_NullString_ReturnsTrue()
    {
        string? s = null;

        var result = s.IsNotNullOrWhiteSpace();

        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase(" ")]
    [TestCase("  ")]
    [TestCase("   ")]
    public void IsNotNullOrWhiteSpace_WhiteSpaceString_ReturnsTrue(
        string s)
    {
        var result = s.IsNotNullOrWhiteSpace();

        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase("a")]
    [TestCase("ab")]
    [TestCase("abc")]
    public void IsNotNullOrWhiteSpace_StringWithCharacter_ReturnsFalse(
        string s)
    {
        var result = s.IsNotNullOrWhiteSpace();

        Assert.That(result, Is.True);
    }
}