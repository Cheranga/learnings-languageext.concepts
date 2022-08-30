using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using LngExt.Learnings.Primal.Tests.Core;
using static LngExt.Learnings.Primal.Tests.Core.TryCatchBoxExtensions;

namespace LngExt.Learnings.Primal.Tests.Customers;

[ExcludeFromCodeCoverage]
public class TestCustomerDataStoreRunTime : ICustomerDataStoreRunTime
{
    private readonly List<Customer> _customers;

    public TestCustomerDataStoreRunTime(List<Customer> customers)
    {
        _customers = customers;
    }

    public Task<Box<Unit>> RegisterCustomerAsync(Customer customer)
    {
        _customers.Add(customer);
        return Task.FromResult(default(Unit).ToPure());
    }

    public async Task<Box<Customer>> UpdateCustomerAsync(
        Expression<Func<Customer, bool>> filter,
        Func<Customer, Customer> updateOperations
    ) =>
        from customer in await GetCustomerAsync(filter)
        from op in Try(() => updateOperations(customer))
        select op;

    public async Task<Box<Customer>> GetCustomerAsync(
        Expression<Func<Customer, bool>> filterExpression
    ) =>
        from customer in await TryAsync(
            async () => await Task.FromResult(_customers.First(x => filterExpression.Compile()(x)))
        )
        select customer;
}
