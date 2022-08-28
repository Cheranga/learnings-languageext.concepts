﻿using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using LngExt.Learnings.Primal.Tests.Core;
using static LngExt.Learnings.Primal.Tests.Core.TryCatchBoxExtensions;

namespace LngExt.Learnings.Primal.Tests.Customers;

public interface ICustomerDataStoreRunTime
{
    Task<Box<Unit>> RegisterCustomerAsync(Customer customer);
    Task<Box<Unit>> UpdateCustomerAsync(
        Expression<Func<Customer, bool>> filter,
        Action<Customer> updateOperations
    );
    Task<Box<Customer>> GetCustomerAsync(Expression<Func<Customer, bool>> filterExpression);
}

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

    public async Task<Box<Unit>> UpdateCustomerAsync(
        Expression<Func<Customer, bool>> filter,
        Action<Customer> updateOperations
    ) =>
        (
            from customer in await GetCustomerAsync(filter)
            from op in Try(() =>
            {
                updateOperations(customer);
                return default(Unit);
            })
            select op
        ).BiMap(unit => unit, Box<Unit>.ToNone);

    public Task<Box<Customer>> GetCustomerAsync(
        Expression<Func<Customer, bool>> filterExpression
    ) =>
        Task.FromResult(
            from customer in Try(() => _customers.First(x => filterExpression.Compile()(x)))
            select customer
        );
}
