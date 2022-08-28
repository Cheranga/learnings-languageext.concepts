using LngExt.Learnings.Primal.Tests.Core;
using static LngExt.Learnings.Primal.Tests.Core.TryCatchBoxExtensions;

namespace LngExt.Learnings.Primal.Tests.Customers;

public static class CustomerOperations
{
    public static Box<Customer> GetCustomerById(
        IEnumerable<Customer> customers,
        string customerId
    ) =>
        from customer in customers
            .Find(x => x.Id.CompareWithoutCase(customerId))
            .MapFail(
                error => Error.New("CustomerNotFound", "customer cannot be found", error.Exception)
            )
        select customer;

    public static Box<Customer> GetCustomerFromService(
        IEnumerable<Customer> customers,
        string customerId
    ) =>
        from customer in Try(() => customers.First(x => x.Id.CompareWithoutCase(customerId)))
            .MapFail(
                error => Error.New("CustomerNotFound", "customer cannot be found", error.Exception)
            )
        select customer;

    public static async Task<Box<Customer>> GetCustomerFromServiceAsync(
        IEnumerable<Customer> customers,
        string customerId
    ) =>
        from customer in (
            await TryAsync(
                () => Task.FromResult(customers.First(x => x.Id.CompareWithoutCase(customerId)))
            )
        ).MapFail(
            error => Error.New("CustomerNotFound", "customer cannot be found", error.Exception)
        )
        select customer;
}
