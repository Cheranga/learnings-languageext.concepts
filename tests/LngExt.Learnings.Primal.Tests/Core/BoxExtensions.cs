namespace LngExt.Learnings.Primal.Tests.Core;

public static class BoxExtensions
{
    private static Box<A> ToNone<A>() => new();

    public static Box<A> ToBox<A>(A value) => value == null ? ToNone<A>() : new Box<A>(value);

    public static bool IsNone<A>(this Box<A> @this) => @this.Data == null;

    public static bool IsSome<A>(this Box<A> @this) => !@this.IsNone();

    public static Box<B> Map<A, B>(this Box<A> @this, Func<A, B> mapper) =>
        @this.IsSome() ? new Box<B>(mapper(@this.Data)) : ToNone<B>();

    public static Box<B> Select<A, B>(this Box<A> @this, Func<A, B> mapper) =>
        @this.IsSome() ? new Box<B>(mapper(@this.Data)) : ToNone<B>();

    public static Unit IfSome<A>(this Box<A> @this, Action<A> action) =>
        @this.IsSome() ? ExecuteAction(@this.Data, action) : default;

    public static Unit IfNone<A>(this Box<A> @this, Action<A> action) =>
        @this.IsNone() ? ExecuteAction(@this.Data, action) : default;

    private static Unit ExecuteAction<A>(A data, Action<A> action)
    {
        action(data);
        return default;
    }

    // public static Box<B> Map<A,B>(this Box<A> @this, Func<A,B> mapper)=>
}