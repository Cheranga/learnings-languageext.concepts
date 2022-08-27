namespace LngExt.Learnings.Primal.Tests.Core;

public static class EnumerableExtensions
{
    public static Box<T> Find<T>(this IEnumerable<T> @this, Predicate<T> filter) =>
        @this == null ? Box<T>.ToNone() : Box<T>.ToSome(@this.FirstOrDefault(x => filter(x)));
}
