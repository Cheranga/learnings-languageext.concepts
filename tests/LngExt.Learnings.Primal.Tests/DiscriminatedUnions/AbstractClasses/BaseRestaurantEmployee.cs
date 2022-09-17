using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace LngExt.Learnings.Primal.Tests.DiscriminatedUnions.AbstractClasses;

public abstract record BaseRestaurantEmployee
{
    private BaseRestaurantEmployee() { }

    [ExcludeFromCodeCoverage(
        Justification = "All the other domain methods, are based on this method."
    )]
    public TB Map<TB>(Func<Chef, TB> chefMapper, Func<Waiter, TB> waiterMapper) =>
        this switch
        {
            Chef c => chefMapper(c),
            Waiter w => waiterMapper(w),
            _ => throw new NotSupportedException()
        };

    [ExcludeFromCodeCoverage(
        Justification = "All the other domain methods, are based on this method."
    )]
    public TB Map<TB>((Func<Chef, TB> chefMapper, Func<Waiter, TB> waiterMapper) mappers) =>
        this switch
        {
            Chef c => mappers.chefMapper(c),
            Waiter w => mappers.waiterMapper(w),
            _ => throw new NotSupportedException()
        };

    public (string, string, string) GetEmployeeInfo() =>
        Map(c => ("Chef", c.Id, c.Name), w => ("Waiter", w.Id, w.Name));

    public DayOfWeek[] GetAvailability() =>
        this switch
        {
            Chef c => c.Availability,
            Waiter w => w.Availability,
            _ => throw new NotSupportedException()
        };

    public bool CanPrepareMeal(string cousine) =>
        !string.IsNullOrEmpty(cousine)
        && Map(
            c =>
                c.Specialities.Any(
                    x => string.Equals(x, cousine, StringComparison.OrdinalIgnoreCase)
                ),
            w => false
        );

    public bool WorksIn(DayOfWeek day) =>
        this switch
        {
            Chef c => c.Availability.Contains(day),
            Waiter w => w.Availability.Contains(day),
            _ => throw new NotSupportedException()
        };

    public record Chef(string Id, string Name, DayOfWeek[] Availability, string[] Specialities)
        : BaseRestaurantEmployee;

    public record Waiter(string Id, string Name, DayOfWeek[] Availability) : BaseRestaurantEmployee;
}

public static class DateTimeExtensions
{
    public static DayOfWeek[] WeekDays =>
        new[]
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        };

    public static DayOfWeek[] WeekEnds => new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

    public static DayOfWeek[] Week =>
        new[]
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };
}

public static class BaseRestaurantEmployeeTests
{
    private static readonly Func<BaseRestaurantEmployee, (string, string, string)> getEmployeeInfo =
        e => e.Map(c => ("Chef", c.Id, c.Name), w => ("Waiter", w.Id, w.Name));

    private static readonly Func<BaseRestaurantEmployee, string, bool> canPrepareMeal = (e, s) =>
        e.Map(c => c.Specialities.Contains(s), _ => false);

    private static readonly Func<BaseRestaurantEmployee, string, bool> canPrepareMealWithTuples = (
        e,
        s
    ) => e.Map((c => c.Specialities.Contains(s), _ => false));

    [Fact]
    public static void GettingEmployeeInfo()
    {
        BaseRestaurantEmployee chef = new BaseRestaurantEmployee.Chef(
            "666",
            "Cheranga",
            DateTimeExtensions.Week,
            new[] { "Sri Lankan" }
        );
        getEmployeeInfo(chef).Should().BeEquivalentTo(("Chef", "666", "Cheranga"));

        chef.GetEmployeeInfo().Should().BeEquivalentTo(("Chef", "666", "Cheranga"));

        BaseRestaurantEmployee waiter = new BaseRestaurantEmployee.Waiter(
            "111",
            "Jon Snow",
            DateTimeExtensions.WeekEnds
        );
        getEmployeeInfo(waiter).Should().BeEquivalentTo(("Waiter", "111", "Jon Snow"));
        waiter.GetEmployeeInfo().Should().BeEquivalentTo(("Waiter", "111", "Jon Snow"));
    }

    [Fact]
    public static void GettingEmployeeInfoWithTuples()
    {
        BaseRestaurantEmployee chef = new BaseRestaurantEmployee.Chef(
            "666",
            "Cheranga",
            DateTimeExtensions.Week,
            new[] { "Sri Lankan" }
        );
        getEmployeeInfo(chef).Should().BeEquivalentTo(("Chef", "666", "Cheranga"));

        BaseRestaurantEmployee waiter = new BaseRestaurantEmployee.Waiter(
            "111",
            "Jon Snow",
            new[] { DayOfWeek.Friday }
        );
    }

    [Fact]
    public static void GettingMealPreparationSpecialities()
    {
        BaseRestaurantEmployee chef = new BaseRestaurantEmployee.Chef(
            "666",
            "Cheranga",
            DateTimeExtensions.Week,
            new[] { "Sri Lankan" }
        );
        BaseRestaurantEmployee waiter = new BaseRestaurantEmployee.Waiter(
            "111",
            "Jon Snow",
            new[] { DayOfWeek.Friday }
        );
        canPrepareMeal(chef, "Thai").Should().BeFalse();
        canPrepareMeal(chef, "Sri Lankan").Should().BeTrue();
        canPrepareMeal(waiter, "any").Should().BeFalse();

        chef.CanPrepareMeal("sri lankan").Should().BeTrue();
        waiter.CanPrepareMeal("Any").Should().BeFalse();
    }

    [Fact]
    public static void GettingMealPreparationSpecialitiesWithTuples()
    {
        BaseRestaurantEmployee chef = new BaseRestaurantEmployee.Chef(
            "666",
            "Cheranga",
            DateTimeExtensions.Week,
            new[] { "Sri Lankan" }
        );
        BaseRestaurantEmployee waiter = new BaseRestaurantEmployee.Waiter(
            "111",
            "Jon Snow",
            new[] { DayOfWeek.Friday }
        );
        canPrepareMealWithTuples(chef, "Thai").Should().BeFalse();
        canPrepareMealWithTuples(chef, "Sri Lankan").Should().BeTrue();
        canPrepareMealWithTuples(waiter, "any").Should().BeFalse();
    }

    [Fact]
    public static void GetAvailabilityTests()
    {
        BaseRestaurantEmployee chef = new BaseRestaurantEmployee.Chef(
            "666",
            "Cheranga",
            DateTimeExtensions.Week,
            new[] { "Western", "Thai" }
        );

        BaseRestaurantEmployee waiter = new BaseRestaurantEmployee.Waiter(
            "888",
            "Jon Snow",
            DateTimeExtensions.WeekEnds
        );

        chef.GetAvailability().Should().BeEquivalentTo(DateTimeExtensions.Week);
        waiter.GetAvailability().Should().BeEquivalentTo(DateTimeExtensions.WeekEnds);
    }

    [Fact]
    public static void WorksInTests()
    {
        BaseRestaurantEmployee chef = new BaseRestaurantEmployee.Chef(
            "666",
            "Cheranga",
            DateTimeExtensions.WeekDays,
            new[] { "Western" }
        );

        BaseRestaurantEmployee waiter = new BaseRestaurantEmployee.Waiter(
            "888",
            "Jon Snow",
            new[] { DayOfWeek.Tuesday }
        );

        chef.WorksIn(DayOfWeek.Friday).Should().BeTrue();
        waiter.WorksIn(DayOfWeek.Tuesday).Should().BeTrue();

        new BaseRestaurantEmployee.Chef(
            "666",
            "Cheranga",
            DateTimeExtensions.WeekDays,
            new[] {"Western"}
        ).WorksIn(DayOfWeek.Friday).Should().BeTrue();

        new BaseRestaurantEmployee.Waiter(
            "888",
            "Jon Snow",
            new[] {DayOfWeek.Tuesday}
        ).WorksIn(DayOfWeek.Tuesday).Should().BeTrue();
    }
}
