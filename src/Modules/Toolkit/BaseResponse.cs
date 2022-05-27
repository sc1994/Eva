namespace Eva.ToolKit;

public record BaseResponse
{
    protected BaseResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; }
    public string RequestId { get; set; } = string.Empty;
}

public record GoodResponse<T> : BaseResponse
{
    protected GoodResponse() : base(true)
    {
    }

    public T? Result { get; set; }
}

public record GoodResponse : GoodResponse<object>
{
    public new object? Result { get; set; }
}

public record BadResponse() : BaseResponse(false)
{
    public string Code { get; set; } = string.Empty;
    public string? SubCode { get; set; }
    public string Title { get; set; } = string.Empty;
    public string[]? Messages { get; set; }
    public Exception? Exception { get; set; }
}