using FluentAssertions;
using LngExt.Learnings.Primal.Tests.Core;

namespace LngExt.Learnings.Primal.Tests.Customers;

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

    [Fact]
    public static void FindingUnExistingCustomerWithTry()
    {
        var customers = Enumerable
            .Range(1, 10)
            .Select(x => new Customer { Id = x.ToString(), Name = $"customer-{x}" });

        var customer = CustomerOperations.GetCustomerFromService(customers, "666");

        customer.IsNone().Should().BeTrue();
        customer.IfNone(error =>
        {
            error.ErrorCode.Should().Be("CustomerNotFound");
            error.ErrorMessage.Should().Be("customer cannot be found");
        });
    }
    
    [Fact]
    public static void FindingExistingCustomerWithTry()
    {
        var customers = Enumerable
            .Range(1, 10)
            .Select(x => new Customer { Id = x.ToString(), Name = $"customer-{x}" });

        var customer = CustomerOperations.GetCustomerFromService(customers, "5");

        customer.IsSome().Should().BeTrue();
        customer.IfSome(c =>
        {
            c.Id.Should().Be("5");
            c.Name.Should().Be("customer-5");
        });
    }
    
    [Fact]
    public static async Task FindingUnExistingCustomerWithTryAsync()
    {
        var customers = Enumerable
            .Range(1, 10)
            .Select(x => new Customer { Id = x.ToString(), Name = $"customer-{x}" });

        var customer = await CustomerOperations.GetCustomerFromServiceAsync(customers, "666");

        customer.IsNone().Should().BeTrue();
        customer.IfNone(error =>
        {
            error.ErrorCode.Should().Be("CustomerNotFound");
            error.ErrorMessage.Should().Be("customer cannot be found");
        });
    }
    
    
    
    [Fact]
    public static async Task FindingExistingCustomerWithTryAsync()
    {
        var customers = Enumerable
            .Range(1, 10)
            .Select(x => new Customer { Id = x.ToString(), Name = $"customer-{x}" });

        var customer = await CustomerOperations.GetCustomerFromServiceAsync(customers, "5");

        customer.IsSome().Should().BeTrue();
        customer.IfSome(c =>
        {
            c.Id.Should().Be("5");
            c.Name.Should().Be("customer-5");
        });
    }

    [Fact]
    public static void SelectingManyExistingCustomersMustPass()
    {
        var customers = Enumerable
            .Range(1, 10)
            .Select(x => new Customer { Id = x.ToString(), Name = $"customer-{x}" });
        
        var filteredCustomers = (from oldestCustomer in customers.Find(x=>x.Id == "1")
            from latestCustomer in customers.Find(x=>x.Id == "10")
            select (oldestCustomer, latestCustomer))
            .IfSome(data =>
            {
                data.oldestCustomer.Should().NotBeNull();
                data.latestCustomer.Should().NotBeNull();
            });
    }
}
