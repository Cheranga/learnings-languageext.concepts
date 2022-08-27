namespace LngExt.Learnings.Primal.Tests.Core;

public class Box<T>
{
    private Box() { }

    public static Box<T> ToSome(T data) => data == null ? ToNone() : new Box<T> { Data = data };

    public static Box<T> ToNone() => new() { Error = Error.New("404", "null data") };

    public static Box<T> ToNone(Error error) => new() { Error = error };

    public T Data { get; private init; }

    public Error Error { get; init; }
}

public class Error
{
    public string ErrorCode { get; private init; } = string.Empty;
    public string ErrorMessage { get; private init; } = string.Empty;
    public Exception Exception { get; private init; }

    private Error() { }

    public static Error New(string errorCode, string errorMessage) =>
        new() { ErrorCode = errorCode, ErrorMessage = errorMessage };

    public static Error New(string errorCode, string errorMessage, Exception exception) =>
        new()
        {
            ErrorCode = errorCode,
            ErrorMessage = errorMessage,
            Exception = exception
        };
}
