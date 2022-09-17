using FluentAssertions;

namespace LngExt.Learnings.Primal.Tests.DiscriminatedUnions.Interfaces;

public interface IRestaurantEmployee
{
    public static IRestaurantEmployee Chef(string Id, string Name, string[] Specialities) =>
        new Chef(Id, Name, Specialities);

    public static IRestaurantEmployee Waiter(string Id, string Name, DayOfWeek[] Days) =>
        new Waiter(Id, Name, Days);

    public TB Match<TB>(Func<Chef, TB> chefMapper, Func<Waiter, TB> waiterMapper) =>
        this switch
        {
            Chef c => chefMapper(c),
            Waiter w => waiterMapper(w),
            _ => throw new NotSupportedException()
        };
}

public record Chef(string Id, string Name, string[] Specialities) : IRestaurantEmployee;

public record Waiter(string Id, string Name, DayOfWeek[] Days) : IRestaurantEmployee;

public record FakeEmployee(string FakeId, string FakeName) : IRestaurantEmployee;

public static class DiscriminatedTests
{
    [Fact]
    public static void RestaurantStaffTests()
    {
        Func<IRestaurantEmployee, string> getId = e =>
            e.Match(chef => chef.Id, waiter => waiter.Id);
        Func<IRestaurantEmployee, DayOfWeek[]> getWorkDays = e =>
            e.Match(
                c => new[] { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                w => w.Days
            );

        var chef = IRestaurantEmployee.Chef("CHEF-666", "Cheranga", new[] { "Pastry", "Gourmet" });

        var waiter = IRestaurantEmployee.Waiter(
            "W-001",
            "Cheranga",
            new[] { DayOfWeek.Friday, DayOfWeek.Saturday }
        );

        var fakeEmployee = new FakeEmployee("666", "Cheranga");

        getId(chef).Should().Be("CHEF-666");
        getId(waiter).Should().Be("W-001");
        Assert.Throws<NotSupportedException>(() => getId(fakeEmployee));

        getWorkDays(chef)
            .Should()
            .BeEquivalentTo(new[] { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday });

        getWorkDays(waiter).Should().BeEquivalentTo(new[] { DayOfWeek.Friday, DayOfWeek.Saturday });

        Assert.Throws<NotSupportedException>(() => getWorkDays(fakeEmployee));
    }
}
