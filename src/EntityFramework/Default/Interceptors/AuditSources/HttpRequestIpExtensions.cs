using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Honamic.Framework.Persistence.EntityFramework.Interceptors.AuditSources;

public static class HttpRequestIpExtensions
{

    public static string GetIP(this HttpContext httpContext, bool tryUseXForwardHeader = true)
    {
        string text = string.Empty;
        if (tryUseXForwardHeader)
        {
            text = SplitCsv(httpContext.GetHeaderValue("X-Forwarded-For")).FirstOrDefault();
        }
        if (string.IsNullOrWhiteSpace(text) && httpContext?.Connection?.RemoteIpAddress != null)
        {
            text = httpContext.Connection.RemoteIpAddress.ToString();
        }
        if (string.IsNullOrWhiteSpace(text))
        {
            text = httpContext.GetHeaderValue("REMOTE_ADDR");
        }
        return text;
    }

    private static string GetHeaderValue(this HttpContext httpContext, string headerName)
    {
        return (httpContext?.Request?.Headers?.TryGetValue(headerName, out StringValues value)).GetValueOrDefault() ? value.ToString() : string.Empty;
    }

    private static List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
    {
        if (string.IsNullOrWhiteSpace(csvList))
        {
            return !nullOrWhitespaceInputReturnsNull ? new List<string>() : null;
        }
        return (from s in csvList.TrimEnd(',').Split(',').AsEnumerable()
                select s.Trim()).ToList();
    }
}