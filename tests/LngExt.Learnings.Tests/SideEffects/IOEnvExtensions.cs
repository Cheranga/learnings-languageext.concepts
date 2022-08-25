namespace LngExt.Learnings.Tests.SideEffects;

public delegate Either<Error, A> IOEnv<Env, A>(Env env);

public static class IOEnvExtensions
{
    public static IOEnv<Env, A> Pure<Env, A>(A value) => env => value;

    public static Either<Error, A> Run<Env, A>(this IOEnv<Env, A> operation, Env env) =>
        Try(() => operation(env))
            .IfFail(exception => Error.New(500, "error when running the IO operation", exception));

    public static IOEnv<Env, B> Select<Env, A, B>(
        this IOEnv<Env, A> operation,
        Func<A, B> mapper
    ) => env => operation(env).Match(a => mapper(a), Left<Error, B>);

    public static IOEnv<Env, B> Map<Env, A, B>(this IOEnv<Env, A> operation, Func<A, B> mapper) =>
        operation.Select(mapper);

    public static IOEnv<Env, B> SelectMany<Env, A, B>(
        this IOEnv<Env, A> operation,
        Func<A, IOEnv<Env, B>> mapper
    ) => env => operation(env).Match(a => mapper(a)(env), Left<Error, B>);

    public static IOEnv<Env, B> Bind<Env, A, B>(
        this IOEnv<Env, A> operation,
        Func<A, IOEnv<Env, B>> mapper
    ) => operation.SelectMany(mapper);

    public static IOEnv<Env, C> SelectMany<Env, A, B, C>(
        this IOEnv<Env, A> operation,
        Func<A, IOEnv<Env, B>> bind,
        Func<A, B, C> project
    ) => operation.SelectMany(a => bind(a).Select(b => project(a, b)));
}
