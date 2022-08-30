using System.Linq.Expressions;
using LngExt.Learnings.Primal.Tests.Core;

namespace LngExt.Learnings.Primal.Tests.Customers;

public interface ICustomerDataStoreRunTime
{
    Task<Box<Unit>> RegisterCustomerAsync(Customer customer);
    Task<Box<Customer>> UpdateCustomerAsync(
        Expression<Func<Customer, bool>> filter,
        Func<Customer, Customer> updateOperations
    );
    Task<Box<Customer>> GetCustomerAsync(Expression<Func<Customer, bool>> filterExpression);
}