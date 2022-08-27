using LngExt.Learnings.Primal.Tests.Core;

namespace LngExt.Learnings.Primal.Tests.Customers;

public static class CustomerOperations
{
    public static Box<Customer> GetCustomerById(
        IEnumerable<Customer> customers,
        string customerId,
        bool ignoreCase = true
    ) =>
        from customer in customers.Find(
            x =>
                string.Equals(
                    x.Id,
                    customerId,
                    ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal
                )
        )
        select customer;
}