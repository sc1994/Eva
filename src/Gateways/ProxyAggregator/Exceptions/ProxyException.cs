using System.Net;

namespace Eva.ProxyAggregator.Exceptions;

public class ProxyException : Exception
{
    public ProxyException(string title, Exception innerProxyException) : base(innerProxyException.Message, innerProxyException)
    {
        Title = title;
    }

    public ProxyException(string title, HttpStatusCode statusCode, string message) : base(message)
    {
        Title = title;
        StatusCode = statusCode;
    }

    public string Title { get; }
    public HttpStatusCode StatusCode { get; }
}