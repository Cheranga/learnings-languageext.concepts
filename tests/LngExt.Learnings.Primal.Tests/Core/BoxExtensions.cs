namespace LngExt.Learnings.Primal.Tests.Core;

public static class BoxExtensions
{
    public static Box<T> ToPure<T>(this T data) =>
        data == null ? Box<T>.ToNone() : Box<T>.ToSome(data);

    public static bool IsNone<A>(this Box<A> @this) => @this.Data == null;

    public static bool IsSome<A>(this Box<A> @this) => !@this.IsNone();

    public static Box<B> Map<A, B>(this Box<A> @this, Func<A, B> mapper) =>
        @this.IsNone() ? Box<B>.ToNone(@this.Error) : Box<B>.ToSome(mapper(@this.Data));

    public static Box<B> Select<A, B>(this Box<A> @this, Func<A, B> mapper) => @this.Map(mapper);

    public static Unit IfSome<A>(this Box<A> @this, Action<A> action) =>
        @this.IsSome() ? ExecuteAction(@this.Data, action) : default;

    public static Unit IfNone<A>(this Box<A> @this, Action<Error> action) =>
        @this.IsNone() ? ExecuteAction(@this.Error, action) : default;

    public static Box<A> MapFail<A>(this Box<A> @this, Func<Error, Error> errorMapper) =>
        @this.IsSome() ? @this : Box<A>.ToNone(errorMapper(@this.Error));

    public static Box<C> SelectMany<A, B, C>(
        this Box<A> @this,
        Func<A, Box<B>> mapper,
        Func<A, B, C> projector
    ) =>
        @this.IsNone()
            ? Box<C>.ToNone()
            : mapper(@this.Data).IsNone()
                ? Box<C>.ToNone()
                : projector(@this.Data, mapper(@this.Data).Data).ToPure();

    private static Unit ExecuteAction<A>(A data, Action<A> action)
    {
        action(data);
        return default;
    }
}
