namespace Eva.ProxyAggregator.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string title, string message = "", string errorCode = "000", Exception? innerException = null) : base(message, innerException)
    {
        Title = title;
        ErrorCode = errorCode;
    }

    public string ErrorCode { get; }

    public string Title { get; }
}