namespace FinAccountMongoApi.Models;

public class OperationResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static OperationResult<T> Ok(T data, string message)
    {
        return new OperationResult<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static OperationResult<T> Fail(string message)
    {
        return new OperationResult<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
}
