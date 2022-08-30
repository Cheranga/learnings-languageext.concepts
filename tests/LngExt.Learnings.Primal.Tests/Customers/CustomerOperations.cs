using System.Linq.Expressions;
using LngExt.Learnings.Primal.Tests.Core;

namespace LngExt.Learnings.Primal.Tests.Customers;

public static class CustomerOperations
{
    public static async Task<Box<Customer>> GetCustomerById(
        ICustomerDataStoreRunTime runTime,
        string customerId
    ) =>
        from customer in (
            await runTime.GetCustomerAsync(customer => customer.Id.CompareWithoutCase(customerId))
        ).MapFail("CustomerNotFound", "customer cannot be found")
        select customer;

    // public static Box<Customer> GetCustomerFromService(
    //     ICustomerDataStoreRunTime runTime,
    //     string customerId
    // ) =>
    //     from customer in Try(() => customers.First(x => x.Id.CompareWithoutCase(customerId)))
    //         .MapFail("CustomerNotFound", "customer cannot be found")
    //     select customer;

    public static async Task<Box<Customer>> GetCustomerFromServiceAsync(
        ICustomerDataStoreRunTime runTime,
        string customerId
    ) =>
        from customer in (
            await runTime.GetCustomerAsync(c => c.Id.CompareWithoutCase(customerId))
        ).MapFail("CustomerNotFound", "customer cannot be found")
        select customer;

    public static async Task<Box<Unit>> RegisterCustomerAsync(
        ICustomerDataStoreRunTime runTime,
        Customer customer
    ) =>
        customer.ToPure().IsNone()
            ? Box<Unit>.ToNone("InvalidCustomer", "invalid customer details")
            : from op in (await runTime.RegisterCustomerAsync(customer)).MapFail(
                "CustomerRegistrationError",
                "error occurred when registering the customer"
            )
            select op;

    public static async Task<Box<Customer>> UpdateCustomerAsync(
        ICustomerDataStoreRunTime runTime,
        Expression<Func<Customer, bool>> filter,
        Func<Customer, Customer> updateOperations
    ) =>
        from op in (await runTime.UpdateCustomerAsync(filter, updateOperations)).MapFail(
            "FailedCustomerUpdate",
            "customer update failed"
        )
        select op;
}
