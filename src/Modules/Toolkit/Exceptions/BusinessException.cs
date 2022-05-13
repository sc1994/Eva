namespace Eva.ToolKit.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string title, string message = "", string code = "500", string subCode = "000", Exception? innerException = null) : base(message, innerException)
    {
        Title = title;
        Code = code;
        SubCode = subCode;
    }

    public string Title { get; }
    public string Code { get; }
    public string SubCode { get; }
}