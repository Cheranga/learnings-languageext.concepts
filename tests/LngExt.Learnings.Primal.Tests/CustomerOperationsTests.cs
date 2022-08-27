using FluentAssertions;
using LngExt.Learnings.Primal.Tests.Core;
using LngExt.Learnings.Primal.Tests.Customers;

namespace LngExt.Learnings.Primal.Tests;

public static class CustomerOperationsTests
{
    [Fact]
    public static void FindExistingCustomerMustPass()
    {
        var customers = Enumerable
            .Range(1, 10)
            .Select(x => new Customer { Id = x.ToString(), Name = $"customer-{x}" });
        var customer = CustomerOperations.GetCustomerById(customers, "5");

        customer.IsSome().Should().BeTrue();
        customer.IfSome(c =>
        {
            c.Id.Should().Be("5");
            c.Name.Should().Be("customer-5");
        });
    }

    [Fact]
    public static void FindUnExistingCustomerMustFail()
    {
        var customers = Enumerable
            .Range(1, 10)
            .Select(x => new Customer { Id = x.ToString(), Name = $"customer-{x}" });
        
        var customer = CustomerOperations.GetCustomerById(customers, "20");

        customer.IsNone().Should().BeTrue();
        customer.IfNone(err =>
        {
            err.ErrorCode.Should().Be("CustomerNotFound");
            err.ErrorMessage.Should().Be("customer cannot be found");
        });
    }

    [Fact]
    public static void FindingCustomerInEmptyCollectionMustFail()
    {
        var customer = CustomerOperations.GetCustomerById(new List<Customer>(), "1");
        customer.IsNone().Should().BeTrue();
    }

    [Fact]
    public static void FindingCustomerInNullCollectionMustFail()
    {
        var customer = CustomerOperations.GetCustomerById(null, "1");
        customer.IsNone().Should().BeTrue();
    }
}
