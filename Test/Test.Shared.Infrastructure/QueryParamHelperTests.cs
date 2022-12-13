using System.Collections.Generic;
using NUnit.Framework;
using Shared.Infrastructure;

namespace Test.Shared.Infrastructure;

[TestFixture]
public class QueryParamHelperTests
{
    [Test]
    public void AddQueryParamToUri_WhnCalled_ReturnsFullUri()
    {
        var result = QueryParamHelper.AddQueryParamToUri(
            "http://a.b",
            new Dictionary<string, object?>
            {
                {
                    "param", "value"
                }
            });

        Assert.That(result, Is.EqualTo("http://a.b/?param=value"));
    }
}