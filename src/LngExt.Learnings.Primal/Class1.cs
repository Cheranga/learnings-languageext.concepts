namespace LngExt.Learnings.Primal;

using static BoxExtensions;
using static EnumerableExtensions;

public class Box<T>
{
    public Box(T data) => Data = data;

    public Box() { }

    public T Data { get; }
}

public static class BoxExtensions
{
    private static Box<A> ToNone<A>() => new();

    public static Box<A> ToBox<A>(A value) => value == null ? ToNone<A>() : new Box<A>(value);

    public static bool IsNone<A>(this Box<A> @this) => @this.Data == null;

    public static bool IsSome<A>(this Box<A> @this) => !@this.IsNone();

    public static Box<B> Map<A, B>(this Box<A> @this, Func<A, B> mapper) =>
        @this.IsSome() ? new Box<B>(mapper(@this.Data)) : ToNone<B>();

    // public static Box<B> Map<A,B>(this Box<A> @this, Func<A,B> mapper)=>
}

public record Customer
{
    public string Id { get; init; }
    public string Name { get; init; }
}

public static class EnumerableExtensions
{
    public static Box<T> Find<T>(this IEnumerable<T> @this, Predicate<T> filter) =>
        @this == null ? new Box<T>() : ToBox(@this.FirstOrDefault(x => filter(x)));
}

public static class CustomerOperations
{
    public static Box<Customer> GetCustomerById(
        IEnumerable<Customer> customers,
        string customerId,
        bool ignoreCase = true
    ) =>
        customers.Find(
            x =>
                string.Equals(
                    x.Id,
                    customerId,
                    ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal
                )
        );
}


