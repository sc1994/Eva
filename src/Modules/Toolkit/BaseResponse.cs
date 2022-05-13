namespace Eva.ToolKit;

public record BaseResponse
{
    public bool Success => true;
    public string RequestId { get; set; } = string.Empty;
}

public record GoodResponse<T> : BaseResponse
{
    public T? Result { get; set; }
}

public record GoodResponse : GoodResponse<object>
{
    public new object? Result { get; set; }
}

public record BadResponse : BaseResponse
{
    public string Code { get; set; } = string.Empty;
    public string? SubCode { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Message { get; set; }
    public Exception? Exception { get; set; }
}