using FluentAssertions;
using LngExt.Learnings.Primal.Tests.Core;

namespace LngExt.Learnings.Primal.Tests.Customers;

public static class CustomerOperationsTests
{
    [Fact]
    public static async Task FindExistingCustomerMustPass()
    {
        var customers = Enumerable
            .Range(1, 10)
            .Select(x => new Customer { Id = x.ToString(), Name = $"customer-{x}" });

        var runTime = new TestCustomerDataStoreRunTime(customers.ToList());
        var customer = await CustomerOperations.GetCustomerById(runTime, "5");

        customer.IsSome().Should().BeTrue();
        customer.IfSome(c =>
        {
            c.Id.Should().Be("5");
            c.Name.Should().Be("customer-5");
        });
    }

    [Fact]
    public static async Task FindUnExistingCustomerMustFail()
    {
        var customers = Enumerable
            .Range(1, 10)
            .Select(x => new Customer { Id = x.ToString(), Name = $"customer-{x}" });

        var runTime = new TestCustomerDataStoreRunTime(customers.ToList());
        var customer = await CustomerOperations.GetCustomerById(runTime, "20");

        customer.IsNone().Should().BeTrue();
        customer.IfNone(err =>
        {
            err.ErrorCode.Should().Be("CustomerNotFound");
            err.ErrorMessage.Should().Be("customer cannot be found");
        });
    }

    [Fact]
    public static async Task FindingCustomerInEmptyCollectionMustFail()
    {
        var runTime = new TestCustomerDataStoreRunTime(new List<Customer>());

        var customer = await CustomerOperations.GetCustomerById(runTime, "1");
        customer.IsNone().Should().BeTrue();
    }

    [Fact]
    public static async Task FindingCustomerInNullCollectionMustFail()
    {
        var runTime = new TestCustomerDataStoreRunTime(null);
        var customer = await CustomerOperations.GetCustomerById(runTime , "1");
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
        
        var filteredCustomers = (from oldestCustomer in customers.FindInCollection(x=>x.Id == "1")
            from latestCustomer in customers.FindInCollection(x=>x.Id == "10")
            select (oldestCustomer, latestCustomer))
            .IfSome(data =>
            {
                data.oldestCustomer.Should().NotBeNull();
                data.latestCustomer.Should().NotBeNull();
            });
    }

    [Fact]
    public static async Task RegisterNonNullCustomerMustPass()
    {
        var customers = Enumerable
            .Range(1, 10)
            .Select(x => new Customer { Id = x.ToString(), Name = $"customer-{x}" });

        var runTime = new TestCustomerDataStoreRunTime(customers.ToList());
        var op = await CustomerOperations.RegisterCustomerAsync(runTime, new Customer {Id = "666", Name = "Cheranga"});

        op.IsSome().Should().BeTrue();
    }
    
    [Fact]
    public static async Task RegisterNullCustomerMustPass()
    {
        var customers = Enumerable
            .Range(1, 10)
            .Select(x => new Customer { Id = x.ToString(), Name = $"customer-{x}" });

        var runTime = new TestCustomerDataStoreRunTime(customers.ToList());
        var op = await CustomerOperations.RegisterCustomerAsync(runTime, null);

        op.IsNone().Should().BeTrue();
        op.IfNone(error => error.ErrorCode.Should().Be("InvalidCustomer"));
    }
}
