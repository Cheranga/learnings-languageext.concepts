namespace LngExt.Learnings.Primal.Tests.Core;
using static BoxExtensions;

public static class TryCatchBoxExtensions
{
    public static Box<T> Try<T>(Func<T> operation)
    {
        try
        {
            return operation().ToPure();
        }
        catch (Exception exception)
        {
            return Box<T>.ToNone(Error.New(exception));
        }
    }

    public static async Task<Box<T>> TryAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return (await operation()).ToPure();
            // return a == null ? Box<T>.ToNone() : Box<T>.ToSome(a);
        }
        catch (Exception exception)
        {
            return Box<T>.ToNone(Error.New(exception));
        }
    }
}
