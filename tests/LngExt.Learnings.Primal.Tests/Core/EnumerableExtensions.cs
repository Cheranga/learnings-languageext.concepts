namespace LngExt.Learnings.Primal.Tests.Core;

public static class EnumerableExtensions
{
    public static Box<T> Find<T>(this IEnumerable<T> @this, Predicate<T> filter) =>
        @this == null ? new Box<T>() : BoxExtensions.ToBox(@this.FirstOrDefault(x => filter(x)));
}