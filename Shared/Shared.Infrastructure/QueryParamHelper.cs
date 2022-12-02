using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Shared.Infrastructure;

public static class QueryParamHelper
{
    public static string AddQueryParamToUri(
        string requestUri,
        Dictionary<string, object?> queryParams = null!)
    {
        var uriBuilder = new UriBuilder(requestUri);
        var queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
        foreach (var parameter in queryParams) queryString[parameter.Key] = parameter.Value?.ToString();

        uriBuilder.Query = queryString.ToString();
        return uriBuilder.ToString();
    }

    public static Dictionary<string, object?> ParametrizeObject<T>(
        this T obj)
    {
        var properties = obj!.GetType()
            .GetProperties()
            .OrderBy(
                propertyInfo => ((ColumnAttribute)propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), false)[0])
                    .Order);

        return properties.ToDictionary(
            propertyInfo =>
                ((ColumnAttribute)propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), false)[0]).Name ??
                propertyInfo.Name,
            propertyInfo => propertyInfo.GetValue(obj));
    }
}