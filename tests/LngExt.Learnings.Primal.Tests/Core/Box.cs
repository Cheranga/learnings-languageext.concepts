namespace LngExt.Learnings.Primal.Tests.Core;

public record Box<T>
{
    private Box(T data) => Data = data;
    private Box(Error error) => Error = error;
    private Box() => Error = Error.New("404", "null data");

    public static Box<T> ToSome(T data) => data == null ? ToNone() : new Box<T>(data);
    public static Box<T> ToNone() => new();
    public static Box<T> ToNone(Error error) => new(error);
    public static Box<T> ToNone(string errorCode, string errorMessage) => ToNone(Error.New(errorCode, errorMessage));

    public readonly T Data;

    public readonly Error Error;
}

public class Error
{
    public readonly string ErrorCode;
    public readonly string ErrorMessage;
    public readonly Exception Exception;

    private Error(string errorCode, string errorMessage, Exception exception)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Exception = exception;
    }

    public static Error New(string errorCode, string errorMessage) => New(errorCode, errorMessage, new Exception(errorMessage));

    public static Error New(string errorCode, string errorMessage, Exception exception) => new Error(errorCode, errorMessage, exception);
}
