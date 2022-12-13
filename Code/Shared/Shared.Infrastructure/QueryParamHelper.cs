using System.Web;

namespace Shared.Infrastructure;

public static class QueryParamHelper
{
    public static string AddQueryParamToUri(
        string requestUri,
        Dictionary<string, object?> queryParams)
    {
        var uriBuilder = new UriBuilder(requestUri);
        var queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
        foreach (var parameter in queryParams) queryString[parameter.Key] = parameter.Value?.ToString();

        uriBuilder.Query = queryString.ToString();
        return uriBuilder.Uri.AbsoluteUri;
    }
}