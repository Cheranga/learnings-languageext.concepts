namespace LngExt.Learnings.Files.WithDelegates;

public delegate Either<Error, A> IO<A>();

public static class IOExtensions
{
    public static IO<A> Pure<A>(A value) => () => value;

    public static Either<Error, A> Run<A>(this IO<A> operation) =>
        Try(() => operation())
            .IfFail(exception => Error.New(500, "error when running the IO operation", exception));

    public static IO<B> Select<A, B>(this IO<A> operation, Func<A, B> mapper) =>
        () => operation().Match(a => mapper(a), Left<Error, B>);

    public static IO<B> Map<A, B>(this IO<A> operation, Func<A, B> mapper) =>
        operation.Select(mapper);

    public static IO<B> SelectMany<A, B>(this IO<A> operation, Func<A, IO<B>> mapper) =>
        () => operation().Match(a => mapper(a)(), Left<Error, B>);

    public static IO<B> Bind<A, B>(this IO<A> operation, Func<A, IO<B>> mapper) =>
        operation.SelectMany(mapper);

    public static IO<C> SelectMany<A, B, C>(
        this IO<A> operation,
        Func<A, IO<B>> bind,
        Func<A, B, C> project
    ) => operation.SelectMany(a => bind(a).Select(b => project(a, b)));
}
