namespace Shared.Models;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    protected Result(bool isSuccess, T? data, string? message)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Success() => new(true, default, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}