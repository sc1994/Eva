using System.Net;

namespace Eva.ToolKit;

public class ServiceRequestException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string Exception { get; } = string.Empty;

    public ServiceRequestException(string message) : base(message)
    {
    }

    public ServiceRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ServiceRequestException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public ServiceRequestException(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public ServiceRequestException(string message, HttpStatusCode statusCode, string exception) : base(message)
    {
        StatusCode = statusCode;
        Exception = exception;
    }
}